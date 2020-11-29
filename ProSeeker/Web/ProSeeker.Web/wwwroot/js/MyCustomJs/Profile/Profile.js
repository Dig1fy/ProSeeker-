var serviceIndex = 0;
function AddMoreServices() {
    
    $("#ServicesContainer").
        append(" <div class='form-group col-md-10'><input type='text' class='form-control col-md-6 col' placeholder='Име на услугата' name='services[" + serviceIndex + "].Name' /> <br /> <textarea type='text' style='margin-top:-1rem' class='form-control col-md-12' placeholder='Описание на услугата' name='services[" + serviceIndex + "].Description'></textarea> </div>");
    serviceIndex++;
}
