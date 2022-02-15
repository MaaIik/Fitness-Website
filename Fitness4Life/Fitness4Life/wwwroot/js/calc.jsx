'use strict'


function BMICalculator () {
    
    function handleSubmit(event) {
        event.preventDefault()
                  
        let weight = document.getElementById('weightInput').value;
        let height = document.getElementById('heightInput').value;
        height = height / 100;
        let calc_bmi = weight / height**2;
        calc_bmi = Math.round(calc_bmi);
        
        document.getElementById('bmi-value').value = calc_bmi
    }

    const style1 = {
        color: "black",
        backgroundColor: "DodgerBlue",
        fontFamily: "Arial"
    };

    const style2 = {
        color: "black",
        backgroundColor: "Yellow",
        fontFamily: "Arial"
    };
    
    return (
                    <div id="calc-card" className="card" >
                        <div id="calc-header" className="card-header text-center" style={style2}> BMI calculator</div>
                        <div id="calc-body" className="card-body" style={style1}>
                            <form className="w-50 m-auto" onSubmit={handleSubmit}>
                
                                <div className="form-group">
                                    <label htmlFor="heightInput">Height [cm]: </label>
                                    <input placeholder="Type your height in cm" id="heightInput" required={true} type="number" className="form-control" />
                                </div>
                                
                                
                                <div className="form-group">
                                    <label htmlFor="weightInput">Weight [kg]: </label>
                                    <input  placeholder="Type your weight in kg" id="weightInput" required={true} type="number" className="form-control"/>
                                </div>
                                
                                
                                <button id="calc-submit" className="btn-success" type="submit">Get BMI</button>
                
                                <div  className="form-group">
                                    <input placeholder="BMI value" className="form-control" id="bmi-value" type="number" />
                                </div>
                            </form>
                        </div>
                        <div id="calc-footer" className="card-footer text-center" style={style2}> BMI: under 18 = underweight, 18-25 = normal, 25-30 = overweight, 30-40 = obese, 40+ = morbidly obese</div>
                    </div>


                )
}

ReactDOM.render(<BMICalculator />, document.getElementById('root'))