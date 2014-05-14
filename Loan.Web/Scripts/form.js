function ResetForm(formID)
{
    $("#" + formID).find("input").val("");
    $("#" + formID).find("select").val("");
    $("#" + formID).find("textarea").val("");
}