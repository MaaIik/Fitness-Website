#!/bin/bash

COURSE="ikt201"

TEST_IMAGE=$COURSE/test
SOLUTION_IMAGE=$COURSE/assignment_$1
CONTAINER_NAME=$COURSE-test

SELENIUM="selenium/standalone-chrome"

error()
{
    echo "$1" >&2
}

# Check if Docker is installed
docker >/dev/null 2>&1

if [[ $? -eq 127 ]]; then
    error "Error: Docker is not installed."
    error "Fix  : See instructions in Canvas for details."
    exit 1
fi

# Check if Docker is working
docker info >/dev/null 2>&1

if [[ $? -ne 0 ]]; then
    # Rerun to show errors
    docker info
    error "Error: Docker failed to run."
    error "Fix  : Is Docker running? If not, start it. If it is ask for help in the lab."
    exit 1
fi

# Check Docker OS type
OS_TYPE=$(docker info --format '{{ .OSType }}')

if [[ "$OS_TYPE" = "windows" ]]; then
    error "Error: Docker is configured for Windows containers. We need Linux containers."
    error "Fix  : Right click Docker and choose 'Switch to Linux containers...'"
    exit 1
fi

# Calculate how many tests will run in parallel, max 8
THREADS=$(docker run --rm alpine nproc)
THREADS=$((THREADS < 8 ? THREADS : 8))

# Check for assignment parameter
if [[ "$1" = "" ]]; then
    echo "Usage: ./test.sh <assignment>"
    exit 0
fi

# Check for valid assignment name
if [[ ! -f "test_assignment_$1.py" ]]; then
    error "Error: Failed to find tests for assignment $1."
    error "Fix  : Check if the assignment number is correct. You can also try running 'git pull' to look for new tests."
    error "Fix  : If the problem persists ask for help in the lab."
    exit 1
fi

# Check for solutions directory
if [[ ! -d "solutions" ]]; then
    error "Error: Failed to find solutions directory."
    error "Fix  : Course preparations might have failed. Ask for help in the lab."
    exit 1
fi

# Check for assignment directory
if [[ ! -d "solutions/assignment_$1" ]]; then
    error "Error: Failed to find assignment_$1 in the solutions directory."
    error "Fix  : Check if the assignment directory name is correct. If it is ask for help in the lab."
    exit 1
fi

# Remove existing screenshots, create new screenshots directory
rm -rf screenshots
mkdir screenshots

# Checks for command errors and prints result message
check()
{
    if [[ $? -eq 0 ]]; then
        echo "$1"
    else
        error "Error: $2"
        exit 1
    fi
}

# Remove dangling images if there are more than 100 images in total
if [[ $(docker images | wc -l) -gt 100 ]]; then
    echo "Cleaning up Docker..."
    docker image prune -f
    echo "Docker cleanup completed successfully." "Failed to complete Docker cleanup."
fi

echo "Looking for Selenium Docker image..."
docker image inspect $SELENIUM >/dev/null

if [[ $? -ne 0 ]]; then
    error "Failed to find Selenium image, downloading..."
    docker pull $SELENIUM
    check "Selenium image downloaded." "Failed to download Selenium image."
else
    echo "Selenium image found."
fi

echo "Building test image..."
docker build . -f Dockerfile.test -t $TEST_IMAGE
check "Docker test image built successfully." "Failed to build Docker test image."

echo "Building solution image..."
docker build . -f Dockerfile.build -t $SOLUTION_IMAGE --build-arg ASSIGNMENT=$1 --build-arg TIMESTAMP="$(date)"
check "Docker solution image built successfully." "Failed to build Docker solution image."

# Configure Selenium hub setup
if [ "$(docker-compose -p $COURSE ps | grep -c Up)" -ne $((THREADS+1)) ]; then
  echo "Removing old Selenium containers..."
  docker-compose -p $COURSE down

  echo "Starting new Selenium containers..."
  docker-compose -p $COURSE up -d --scale chrome=$THREADS

  echo "Waiting for Selenium containers to be ready..."
  sleep 10
fi

# Git Bash Docker workaround
export MSYS_NO_PATHCONV=1

# Run tests
docker run --rm --init --name $CONTAINER_NAME \
    -v /var/run/docker.sock:/var/run/docker.sock \
    -v "$(pwd)/screenshots":/results/screenshots \
    --network ${COURSE}_default \
    -e NETWORK=${COURSE}_default \
    -e HUB=selenium-hub \
    -e IMAGE=$SOLUTION_IMAGE \
    $TEST_IMAGE -n $THREADS test_assignment_$1.py $2 $3 $4 $5 $6 $7 $8 $9
