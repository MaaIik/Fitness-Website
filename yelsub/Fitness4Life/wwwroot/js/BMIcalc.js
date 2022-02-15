
function getbmivalue() {

    let weight = document.getElementById('weight').value;
    let height = document.getElementById('height').value;

    height = height / 100;

    let calc_bmi = weight / height ** 2;

    calc_bmi = Math.round(calc_bmi);
}

    document.getElementById('bmi-value').value = calc_bmi


