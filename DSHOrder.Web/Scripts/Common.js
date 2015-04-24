function BindSelect(controlId, url, params)
{
    var control = $("select[id$='" + controlId + "']");
    control.empty();

    $.getJSON(url, params, function (data) {  
        $.each(data, function (i, item) {
            $("<option></option>").val(item["Value"]).text(item["Text"]).appendTo(control);
        });  
    });     
}
