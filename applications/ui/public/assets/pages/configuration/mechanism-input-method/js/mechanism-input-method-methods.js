'use strict';

let swalTitle = "Mechanism Input Method Configuration";
let dt_list;
heightContainer = 390;
let validator;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    validator = FormValidation.formValidation(document.querySelector("#form_upload"), {
        fields: {
            file: {
                selector: '[data-stripe="file"]',
                validators: {
                    notEmpty: {
                        message: 'Choose file..',
                    },
                },
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    });

    dt_list = $('#dt_list').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[0, 'asc']],
        ajax: {
            url: '/configuration/mechanism-input-method/list',
            type: 'get',
        },
        ordering: false,
        processing: true,
        paging: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Category',
                data: 'categoryDesc',
                className: 'text-nowrap align-middle me-5',
            },
            {
                targets: 1,
                title: 'Sub Category',
                data: 'subCategoryDesc',
                className: 'text-nowrap align-middle me-5',
            },
            {
                targets: 2,
                title: 'Activity',
                data: 'activityDesc',
                className: 'text-nowrap align-middle me-5',
            },
            {
                targets: 3,
                title: 'Sub Activity',
                data: 'subActivityDesc',
                className: 'text-nowrap align-middle me-5',
            },
            {
                targets: 4,
                title: 'Using Mechanism List',
                data: 'inputMethod',
                className: 'text-nowrap align-middle me-5',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    $('#dt_list_search').on('keyup', function () {
        dt_list.column(0).search(this.value, false, false).draw();
    });

});

$('#btn_download_template').on('click', function () {
    let url = "/configuration/mechanism-input-method/download-template";

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_upload').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_upload");
            let formData = new FormData($('#form_upload')[0]);
            let url = '/configuration/mechanism-input-method/upload';
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;
            $.ajax({
                url: url,
                data: formData,
                type: 'POST',
                async: true,
                dataType: 'JSON',
                cache: false,
                contentType: false,
                processData: false,
                beforeSend: function () {
                },
                success: function (result) {
                    if (!result.error) {
                        Swal.fire({
                            title: swalTitle,
                            html: 'Upload Success!',
                            icon: "success",
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        });
                        $('#form_upload')[0].reset();
                        dt_list.ajax.reload();
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
                error: function (jqXHR) {
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
