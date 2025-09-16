'use strict';

var dt_file, dt_upload_history, validator;
var swalTitle = "Transfer to SAP - Accrual";
let dialerObject;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    $('#filter_closing_date').flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "Y-m-d",
        disableMobile: "true",
        altInput: "true",
        allowInput: "true"
    });

    disableButtonSave();
    Promise.all([ getEntity() ]).then(async () => {
        enableButtonSave();
        $('#filter_period').val(new Date().getFullYear());
        dialerObject.setValue(new Date().getFullYear());
    });

    validator = FormValidation.formValidation(document.querySelector("#form_sap_accrual"), {
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

    dt_file = $('#dt_file').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        order: [[5, 'desc']],
        ajax: {
            url: '/tools/sap-accrual/list/report-header?period=' + $('#filter_period').val() + "&entityId=" + ($('#filter_entity').val() ?? "0") + "&closingDate=" + $('#filter_closing_date').val(),
            type: 'get',
        },
        processing: true,
        serverSide: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "23vh",
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 200,
                title: 'Download',
                className: 'text-nowrap align-middle text-center',
                render: function (data, type, full, meta) {
                    return '<button class="btn btn-sm btn-icon btn_accrual_download me-3" title="Generate Accrual"><i class="fa fa-download" style="color: #5867dd"></i></button>' +
                        '<button class="btn btn-sm btn-icon btn_accrual_reversal" title="Generate Accrual Reversal"><i class="fa fa-retweet" style="color: #5867dd"></i></button>'
                }
            },
            {
                targets: 1,
                title: 'User',
                data: 'userId',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Periode',
                data: 'periode',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Entity',
                data: 'entity',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Closing Date',
                data: 'closingDt',
                width: 200,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return formatDate(data);
                }
            },
            {
                targets: 5,
                title: 'Generate On',
                data: 'createOn',
                width: 300,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return formatDateTime(data);
                }
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    dt_upload_history = $('#dt_upload_history').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[0, 'asc']],
        ajax: {
            url: '/tools/sap-accrual/get-list/upload-history',
            type: 'get',
        },
        processing: true,
        serverSide: false,
        paging: true,
        searching: true,
        scrollX: true,
        scrollY: "14vh",
        lengthMenu: [[10, 20, 50, 100, -1], [10, 20, 50, 100, "All"]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Upload On',
                data: 'uploadOn',
                className: 'text-nowrap align-middle',
                width: 250,
                render: function (data, type, full, meta) {
                    return formatDateTime(data);
                }
            },
            {
                targets: 1,
                title: 'Upload By',
                data: 'uploadBy',
                className: 'text-nowrap align-middle',
                width: 250,
            },
            {
                targets: 2,
                title: 'Filename',
                data: 'fileName',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {
        },
        drawCallback: function( settings, json ) {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        let strmessage = e.jqXHR.responseJSON.message
        if(strmessage==="") strmessage = "Please contact your vendor"
        if (e.jqXHR.status !== 200) {
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
        }
    };

    $('#dt_file_search').on('keyup', function () {
        dt_file.search(this.value).draw();
    });

    $('#dt_upload_history_search').on('keyup', function () {
        dt_upload_history.search(this.value).draw();
    });
});

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2019) {
        $(this).val("2019");
        dialerObject.setValue(2019);
    }
});

$('#filter_collapsible').on('hidden.bs.collapse', function () {
    $('div.dataTables_scrollBody').height( "63vh" );
})

$('#filter_collapsible').on('shown.bs.collapse', function () {
    $('div.dataTables_scrollBody').height( "55vh" );
})

$('#filter_period').on('change', function() {
    let period = this.value;
    let month = String(new Date().getMonth()).padStart(2, '0');
    let date = String(new Date().getDate()).padStart(2, '0');

    let endPeriod = formatDate(new Date(period, month, date));
    $('#filter_closing_date').flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "Y-m-d",
        disableMobile: "true",
        altInput: "true",
        allowInput: "true"
    }).setDate(endPeriod, true);
});

$("#dt_file").on('click', '.btn_accrual_download', function () {
    let tr = this.closest("tr");
    let trdata = dt_file.row(tr).data();

    let url = '';
    if (trdata.entity == 'Nutricia Medical Nutrition') {
        downloadURL("/tools/sap-accrual/download-accrual-nmn?id=" + '1' + trdata.id);
        downloadURL("/tools/sap-accrual/download-accrual?id=" + '1' + trdata.id + "&entity=" + trdata.entityId);
    } else {
        url = "/tools/sap-accrual/download-accrual?id=" + '1' + trdata.id + "&entity=" + trdata.entityId;
        downloadURL(url, 0);
    }
});

$("#dt_file").on('click', '.btn_accrual_reversal', function () {
    let tr = this.closest("tr");
    let trdata = dt_file.row(tr).data();

    let url = '';
    if (trdata.entity == 'Nutricia Medical Nutrition') {
        downloadURL("/tools/sap-accrual/download-accrual-nmn?id=" + '2' + trdata.id);
        downloadURL("/tools/sap-accrual/download-reversal?id=" + '2' + trdata.id + "&entity=" + trdata.entityId);
    } else {
        url = "/tools/sap-accrual/download-reversal?id=" + '2' + trdata.id + "&entity=" + trdata.entityId;

        downloadURL(url, 0);
    }
});

const downloadURL = function downloadURL(url, count) {
    var hiddenIFrameID = 'hiddenDownloader' + count;
    var iframe = document.createElement('iframe');
    iframe.id = hiddenIFrameID;
    iframe.style.display = 'none';
    document.body.appendChild(iframe);
    iframe.src = url;
}

$('#btn_export_excel').on('click', function() {
    window.open("/master/promo-mechanism/export-xls", "_blank");
});

$('#dt_file_view').on('click', function() {
    let btn = document.getElementById('dt_file_view');
    let filter_period = $('#filter_period').val();
    let filter_entity = $('#filter_entity').val();
    let filter_closing_date = $('#filter_closing_date').val();

    if(filter_entity == '') {
        filter_entity = 0;
    } else {
        $('#filter_entity').val();
    }
    let url = '/tools/sap-accrual/list/report-header?period=' + filter_period + "&entityId=" + filter_entity + "&closingDate=" + filter_closing_date;
    $.ajax({
        url: url,
        type: 'GET',
        async: true,
        dataType: 'JSON',
        cache: false,
        contentType: false,
        processData: false,
        beforeSend: function () {
            btn.setAttribute("data-kt-indicator", "on");
            btn.disabled = !0;
        },
        success: function (result, status, xhr, $form) {
            if (!result.error) {
                dt_file.clear().draw();
                dt_file.rows.add(result.data).draw();
            } else {
                dt_file.clear().draw();
            }
        },
        complete: function () {
            btn.setAttribute("data-kt-indicator", "off");
            btn.disabled = !1;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(jqXHR.message)
        }
    });
});

$('#dt_upload_history_view').on('click', function() {
    dt_upload_history.ajax.reload();
});

$('.field_upload').on('change', function() {
    let file_size = $(this)[0].files[0].size;
    if (file_size > 10000000) {
        Swal.fire({
            title: "Mechanism",
            text: "Ukuran file lebih dari 10Mb, mohon pilih file yang ukurannya kurang dari 10Mb",
            icon: "warning",
            confirmButtonText: "Ok",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: { confirmButton: "btn btn-optima" }
        }).then(function () {
            $('.field_upload').val(null);
            validator.resetForm(true);
        });
    }
});

$('#btn_upload').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_upload");
            var formData = new FormData($('#form_sap_accrual')[0]);
            let url = '/tools/sap-accrual/upload-xml';
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
                    e.setAttribute("data-kt-indicator", "on");
                    e.disabled = !0;
                },
                success: function (result, status, xhr, $form) {
                    if (!result.error) {
                        Swal.fire({
                            title: 'Files Uploaded!',
                            icon: "success",
                            confirmButtonText: "Ok",
                            allowOutsideClick: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(async function () {
                            dt_upload_history.ajax.reload();
                        });
                    } else {
                        Swal.fire({
                            title: swalTitle,
                            text: result.message,
                            icon: "warning",
                            confirmButtonText: "Ok",
                            allowOutsideClick: false,
                            customClass: {confirmButton: "btn btn-optima"}
                        }).then(async function () {
                            dt_upload_history.ajax.reload();
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

const getEntity= () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/tools/sap-payment/get-list/entity",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                var data = [];
                for (var j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#filter_entity').select2({
                    placeholder: "Select a Entity",
                    width: '100%',
                    data: data
                });
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
