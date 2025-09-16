'use strict';
var dt_exception;
var validator_upload ;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    const form = document.getElementById('form_exception');

    validator_upload  = FormValidation.formValidation(form, {
        fields: {
            file: {
                validators: {
                    notEmpty: {
                        message: "Choose File!"
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    })

    dt_exception = $('#dt_exception').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ajax: {
            url: '/fin-rpt/submission/get-data/exception',
            type: 'get',
        },
        processing: true,
        serverSide: true,
        searching: true,
        paging: false,
        scrollCollapse: true,
        scrollX: true,
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'promoid',
                title: 'id',
                visible: false,
                searchable: false,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'Promo ID',
                data: 'promorefid',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Reason',
                data: 'reason',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strmessage = e.jqXHR.responseJSON.message
        if(strmessage==="") strmessage = "Please contact your vendor"
        Swal.fire({
            text: strmessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-optima"},
            closeOnConfirm: false,
            showLoaderOnConfirm: false,
            closeOnClickOutside: false,
            closeOnEsc: false,
            allowOutsideClick: false,
        });
    };
});

$('#dt_exception_search').on('keyup', function () {
    dt_profile.search(this.value).draw();
});

$('#download_template').on('click', function() {
    let url ="/fin-rpt/submission/download-template";

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#uploadxls').on('click', function() {
    let e = document.querySelector("#uploadxls");
    validator_upload.validate().then(function (status) {
        if (status === "Valid") {
            var formData = new FormData($('#form_exception')[0]);
            let url = '/fin-rpt/submission/upload-xls';
            $.ajax({
                url: url,
                data: formData,
                type: 'POST',
                async: true,
                dataType: 'JSON',
                cache: false,
                contentType: false,
                processData: false,
                // enctype: "multipart/form-data",
                beforeSend: function () {
                    dt_exception.clear().draw();
                    e.setAttribute("data-kt-indicator", "on");
                    e.disabled = !0;
                },
                success: function (result, status, xhr, $form) {
                    if (!result.error) {
                        Swal.fire({
                            title: "Files Uploaded!",
                            icon: "success",
                            confirmButtonText: "Ok",
                            allowOutsideClick: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(async function () {
                            dt_exception.clear().draw();
                            dt_exception.ajax.url("/fin-rpt/submission/get-data/exception").load();
                        });
                    } else {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "warning",
                            confirmButtonText: "Ok",
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                    }
                },
                complete: function () {
                    e.setAttribute("data-kt-indicator", "off");
                    e.disabled = !1;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log(jqXHR.message)
                    Swal.fire({
                        title: swalTitle,
                        text: "Files Upload Failed",
                        icon: "error",
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    });
                }
            });
        } else {
            Swal.fire({
                title: swalTitle,
                text: 'Choose file...',
                icon: "warning",
                confirmButtonText: "Ok",
                customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
});

