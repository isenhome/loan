//title：提示框标题，默认为“提示信息”；message:提示信息具体内容
function ShowDialog(message) {
    $('body').append('<div class="modal fade" id="message-dialog"><div class="modal-dialog modal-small"><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title">提示信息</h4></div><div class="modal-body">' + message + '</div><div class="modal-footer"><button id="messageDialog-close" type="button" class="btn btn-primary" data-dismiss="modal">关闭</button></div></div></div></div>');
    $("#message-dialog").modal('show');
}
//title：提示框标题；message:提示信息具体内容
function ShowDialog_Title(title, message) {
    $('body').append('<div class="modal fade" id="message-dialog"><div class="modal-dialog modal-small"><div class="modal-content"><div class="modal-header"><button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button><h4 class="modal-title">' + title + '</h4></div><div class="modal-body">' + message + '</div><div class="modal-footer"><button id="messageDialog-close" type="button" class="btn btn-primary" data-dismiss="modal">关闭</button></div></div></div></div>');
    $("#message-dialog").modal('show');
}