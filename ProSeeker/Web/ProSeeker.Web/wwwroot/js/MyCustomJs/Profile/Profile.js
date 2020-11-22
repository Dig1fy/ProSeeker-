var serviceIndex = 0;
function AddMoreServices() {
    $("#ServicesContainer").
        append("<div class= 'form-control'> Name: <input type='text' name='services[" + serviceIndex + "].Name' />  <hr /><br> Description: <textarea type='text' name='services[" + serviceIndex + "].Description'></textarea> </div > <br>");
    serviceIndex++;
}
