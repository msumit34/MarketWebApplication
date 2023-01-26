class EquityService
{
      
    constructor()
    {
        this.equityApiURL = "https://localhost:7116/Equity/stockData/";
    }

    getTicker() 
    {
        try
        {
            var symbol = document.getElementById("ticker").innerText.split(","); 
            fetch(this.equityApiURL + symbol,{
                method:'GET', 
                headers:{
                    'Content-Type':'application/json',
                    'Access-Control-Allow-Origin':'https://localhost:7116', 
                    'Access-Control-Allow-Credentials':'true'    
                }
            }).then(response =>{
                if(response.body != null)
                {
                   /**  var res = JSON.parse(response.json());
                    document.getElementById("symbol").innerText = res['symbol'];
                    document.getElementById("Last Price").innerText = res['symbol'];
                    document.getElementById("Current Ratio").innerText = res['currentRatio'];
                    document.getElementById("Quick Ratio").innerText = res['quickRatio'];
                    document.getElementById("RSI").innerText = res['FiveDayRelationalStrength'];
                    document.getElementById("TwoHundredDayMovingAverage").innerText = res['TwoHundredDayMovingAverage'];
                    document.getElementById("FiftyDayMovingAverage").innerText = res['FiftyDayMovingAverage'];
                    document.getElementById("FiveDayRelationalStrength").innerText = res['FiveDayRelationalStrength'];
                    document.getElementById("FiftyDayRelationalStrength").innerText = res['FiftyDayRelationalStrength'];**/
                }
            });
        }
        catch(e)
        {
            console.log(e);
        }
    }
}

equityService = new EquityService();
