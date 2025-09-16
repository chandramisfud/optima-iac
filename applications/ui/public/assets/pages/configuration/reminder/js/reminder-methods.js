'use strict';

var validator, method, usergroupid, usergroupname;
var swalTitle = "Approval Reminder Configuration";
var dialerElPreCautionLimitFrom, dialerObjPreCautionLimitFrom;
var dialerElPreCautionLimitTo, dialerObjPreCautionLimitTo;
var dialerElPreCautionNotifFrequency, dialerObjPreCautionNotifFrequency;
var dialerElWarningLimitFrom, dialerObjWarningLimitFrom;
var dialerElWarningLimitTo, dialerObjWarningLimitTo;
var dialerElWarningNotifFrequency, dialerObjWarningNotifFrequency;
var dialerElDangerLimit, dialerObjDangerLimit;
var dialerElDangerNotifFrequency, dialerObjDangerNotifFrequency;


(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    KTDialer.createInstances();
    dialerElPreCautionLimitFrom = document.querySelector("#dialer_pre_caution_limit_from");
    dialerObjPreCautionLimitFrom = new KTDialer(dialerElPreCautionLimitFrom, {
        min: 0,
        step: 1,
    });

    dialerElPreCautionLimitTo = document.querySelector("#dialer_pre_caution_limit_to");
    dialerObjPreCautionLimitTo = new KTDialer(dialerElPreCautionLimitTo, {
        min: 0,
        step: 1,
    });

    dialerElPreCautionNotifFrequency = document.querySelector("#dialer_pre_caution_notif_frequency");
    dialerObjPreCautionNotifFrequency = new KTDialer(dialerElPreCautionNotifFrequency, {
        min: 0,
        step: 1,
    });

    dialerElWarningLimitFrom = document.querySelector("#dialer_warning_limit_from");
    dialerObjWarningLimitFrom = new KTDialer(dialerElWarningLimitFrom, {
        min: 0,
        step: 1,
    });

    dialerElWarningLimitTo = document.querySelector("#dialer_warning_limit_to");
    dialerObjWarningLimitTo = new KTDialer(dialerElWarningLimitTo, {
        min: 0,
        step: 1,
    });

    dialerElWarningNotifFrequency = document.querySelector("#dialer_warning_notif_frequency");
    dialerObjWarningNotifFrequency = new KTDialer(dialerElWarningNotifFrequency, {
        min: 0,
        step: 1,
    });

    dialerElDangerLimit = document.querySelector("#dialer_danger_limit");
    dialerObjDangerLimit = new KTDialer(dialerElDangerLimit, {
        min: 0,
        step: 1,
    });

    dialerElDangerNotifFrequency = document.querySelector("#dialer_danger_notif_frequency");
    dialerObjDangerNotifFrequency = new KTDialer(dialerElDangerNotifFrequency, {
        min: 0,
        step: 1,
    });

    blockUI.block();
    Promise.all([getDataConfig()]).then(function () {
        blockUI.release();
    });
});

$('#pre_caution_from, #pre_caution_to, #pre_caution_frequency, #warning_from, #warning_to, #warning_frequency, #critical_days, #critical_frequency').on('keyup', function () {
    let field = $(this);
    if (field.val() === "") {
        field.val('0');
    }
});

$('#btn_save').on('click', function () {
    let e = document.querySelector("#btn_save");
    let data = [{
        id: 1,
        daysfrom: $('#pre_caution_from').val(),
        daysto: $('#pre_caution_to').val(),
        frequency: $('#pre_caution_frequency').val()
    },{
        id: 2,
        daysfrom: $('#warning_from').val(),
        daysto: $('#warning_to').val(),
        frequency: $('#warning_frequency').val()
    },{
        id: 3,
        daysfrom: $('#critical_days').val(),
        daysto: $('#critical_days').val(),
        frequency: $('#critical_frequency').val()
    }];
    let formData = new FormData();
    formData.append('configList', JSON.stringify(data));
    let url = "/configuration/reminder/update";
    $.ajax({
        url         : url,
        data        : formData,
        type        : 'POST',
        async       : true,
        dataType    : 'JSON',
        cache       : false,
        contentType : false,
        processData : false,
        beforeSend: function() {
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;
        },
        success: function(result, status, xhr, $form) {
            if (!result.error) {
                Swal.fire({
                    title: swalTitle,
                    text: result.message,
                    icon: "success",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                }).then(function () {
                    window.location.href = '/configuration/reminder';
                });
            } else {
                Swal.fire({
                    title: swalTitle,
                    text: result.message,
                    icon: "error",
                    confirmButtonText: "OK",
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
        },
        complete: function() {
            e.setAttribute("data-kt-indicator", "off");
            e.disabled = !1;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.message)
            Swal.fire({
                title: swalTitle,
                text: "Failed to save data, an error occurred in the process",
                icon: "error",
                confirmButtonText: "OK",
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
});

const getDataConfig = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/configuration/reminder/get-data",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                if (!result.error) {
                    let values = result.data;
                    for (let i=0; i<values.length; i++) {
                        if (values[i].id === 1) {
                            $('#pre_caution_from').val(values[i].daysfrom);
                            dialerObjPreCautionLimitFrom.setValue(values[i].daysfrom);
                            $('#pre_caution_to').val(values[i].daysto);
                            dialerObjPreCautionLimitTo.setValue(values[i].daysto);
                            $('#pre_caution_frequency').val(values[i].frequency);
                            dialerObjPreCautionNotifFrequency.setValue(values[i].frequency);
                            $('#pre_caution_info_update').text("updated by " + values[i].useredit + " on " + formatDateTime(values[i].dateedit));
                        } else if (values[i].id === 2) {
                            $('#warning_from').val(values[i].daysfrom);
                            dialerObjWarningLimitFrom.setValue(values[i].daysfrom);
                            $('#warning_to').val(values[i].daysto);
                            dialerObjWarningLimitTo.setValue(values[i].daysto);
                            $('#warning_frequency').val(values[i].frequency);
                            dialerObjWarningNotifFrequency.setValue(values[i].frequency);
                        }  else if (values[i].id === 3) {
                            $('#critical_days').val(values[i].daysfrom);
                            dialerObjDangerLimit.setValue(values[i].daysfrom);
                            $('#critical_frequency').val(values[i].frequency);
                            dialerObjDangerNotifFrequency.setValue(values[i].frequency);
                        }
                    }
                } else {
                    Swal.fire({
                        title: swalTitle,
                        text: result.message,
                        icon: "warning",
                        buttonsStyling: !1,
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            },
            complete: function() {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}
