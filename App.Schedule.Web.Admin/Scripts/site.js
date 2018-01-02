﻿function ValidateForm(btnSubmit) {
    var $form = $(btnSubmit).parent('form');
    var validator = $form.validate();

    $form.find('input').each(function () {
        if (!validator.element(this)) {
            return false;
        }
    });
    return true;
}

function onSubmit(btnSubmit, target) {
    if (!ValidateForm(btnSubmit))
        return false;

    var btnSubmitvar = $(btnSubmit);

    var $form = btnSubmitvar.parents('form');

    btnSubmitvar.attr("disabled", "disabled");

    var errorDiv = $("#errorBox");
    var err_head = $("#err_head");
    var err_title = $("#err_title");
    var err_process = $("#err_process");

    err_head.text("Info: ");
    err_title.text("Please wait...");
    errorDiv.show();
    err_process.css("display", "inline");
    $.ajax({
        type: "POST",
        url: $form.attr('action'),
        data: $form.serialize(),
        error: function (response) {
            err_head.text("Error: ");
            err_title.text(data.message);
            errorDiv.show();
            btnSubmitvar.removeAttr("disabled");
            err_process.css("display", "none");
        },
        success: function (data) {
            btnSubmitvar.removeAttr('disabled');
            console.log(data);
            console.log(JSON.stringify(data));

            if (data.status) {
                err_head.text("Info: ");
                err_title.text("Successed! Please wait...");
                setTimeout(function () {
                    errorDiv.hide();
                    location.href = target;
                    err_process.css("display", "none");
                }, 3000);
            }
            else {
                err_head.text("Info: ");
                err_title.text(data.message);
                errorDiv.show();
                err_process.css("display", "none");
            }
        }
    });
}

function onUpdate(btnSubmit, Id, url, refreshCallback) {

    var btnSubmitvar = $(btnSubmit);

    var $form = btnSubmitvar.parents('form');

    btnSubmitvar.attr("disabled", "disabled");

    var errorDiv = $("#errorBox");
    var err_head = $("#err_head");
    var err_title = $("#err_title");

    err_head.text("Info: ");
    err_title.text("Please wait...");
    errorDiv.show();
    var options = { "backdrop": "static", keyboard: true };
    $.ajax({
        url: url, //"@Url.Action("Edit","Devices")?Id="+Id,
        data: { Id: Id },
        success: function (data) {
            btnSubmitvar.removeAttr('disabled');
            if (data.status) {
                errorDiv.text("Successed! Please wait...");
                setTimeout(function () {
                    //errorDiv.hide();
                    location.href = target;
                }, 500);
            }
            else {
                err_head.text("Info: ");
                err_title.text(data.message);
                errorDiv.show();
            }
        },
        error: function (response) {
            err_head.text("Error: ");
            err_title.text(data.message);
            errorDiv.show();
            btnSubmitvar.removeAttr("disabled");
        }
    });
}
