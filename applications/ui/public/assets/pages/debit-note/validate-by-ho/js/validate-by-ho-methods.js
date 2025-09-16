'use strict';

var dt_validate_by_ho, validator, arrChecked = [], sumAmount = 0, checkFlag = [];
var swalTitle = "Debit Note [Validate By HO]";
heightContainer = 305;

$(document).ready(function () {
    $('#form-upload').hide();
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    validator = FormValidation.formValidation(document.querySelector("#form_upload_validate"), {
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

    dt_validate_by_ho = $('#dt_validate_by_ho').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row'<'col-sm-12'<'dt-footer'>>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        ajax: {
            url: '/dn/validate-by-ho/list',
            type: 'get',
        },
        processing: true,
        serverSide: false,
        paging: true,
        searching: true,
        scrollX: true,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        order: [[1, 'asc']],
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 10,
                searchable: false,
                className: 'text-nowrap dt-body-start text-center align-middle',
                checkboxes: {
                    'selectRow': false,
                },
                createdCell:  function (td, cellData, rowData, row, col){
                    if (checkFlag.length > 0) {
                        if (checkFlag.includes(rowData.id)) {
                            this.api().cell(td).checkboxes.select();
                        } else {
                            this.api().cell(td).checkboxes.deselect();
                        }
                    }
                },
                render: function (data, type, full, meta) {
                    if (type === 'display') {
                        if (full.docCount === 0) {
                            data = '<input type="checkbox" class="dt-checkboxes form-check-input m-1" autocomplete="off" disabled>'
                        } else {
                            data = '<input type="checkbox" class="dt-checkboxes form-check-input m-1" autocomplete="off">';
                        }
                    }
                    return data;
                }
            },
            {
                targets: 1,
                title: '',
                data: 'id',
                width: 10,
                className: 'text-nowrap dt-body-start text-center align-middle',
                render: function (data, type, full, meta) {
                    if (full.docCount === 0) {
                        return '';
                    } else {
                        return '<div class="me-0">' +
                            '<a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">' +
                            '<i class="la la-cog fs-3 text-optima text-hover-optima"></i>' +
                            '</a>' +
                            '<div class="menu menu-sub menu-sub-dropdown w-180px w-md-180px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">' +
                            '<a class="dropdown-item text-start btn-approval" href="/dn/validate-by-ho/approval?id=' + data + '"><i class="fa fa-check fs-6 me-2"></i> DN Approval</a>' +
                            '</div>' +
                            '</div>';
                    }

                }
            },
            {
                targets: 2,
                title: 'Dn Number',
                data: 'refId',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Promo ID',
                data: 'promoRefId',
                width: 120,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'DN Description',
                data: 'activityDesc',
                width: 500,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'DPP',
                data: 'dpp',
                width: 200,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatMoney(data,0);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 6,
                title: 'Last Status',
                data: 'lastStatus',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Category',
                data: 'dnCategory',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 8,
                title: 'Total Claim',
                data: 'totalClaim',
                width: 200,
                visible: false,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatMoney(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 9,
                title: 'Doc Count',
                data: 'docCount',
                visible: false,
                className: 'text-nowrap align-middle text-end',
            },
        ],
        initComplete: function (settings, json) {
            $('#dt_validate_by_ho_wrapper').on('change', 'thead th #dt-checkbox-header', function () {
                let data = dt_validate_by_ho.rows( { filter : 'applied'} ).data();
                arrChecked = [];
                if (this.checked) {
                    for (let i = 0; i < data.length; i++) {
                        if (data[i].docCount !== 0) {
                            arrChecked.push({
                                id: data[i].id,
                                totalClaim: parseFloat(data[i].totalClaim)
                            });
                        }
                    }
                } else {
                    arrChecked = [];
                }
                sumAmount = 0;
                if (arrChecked.length > 0) {
                    for (let i = 0; i < arrChecked.length; i++) {
                        sumAmount += parseFloat(arrChecked[i].totalClaim);
                    }
                }
                $('#tot').html(formatMoney(sumAmount.toString(), 0));
                $('#count').html(arrChecked.length);
            });
        },
        drawCallback: function (settings, json) {
            KTMenu.createInstances();
        },
    });

    $('.dt-footer').html("<div>Total Selected : <span id='count'>0</span>&nbsp&nbsp&nbsp Total Claim : <span id='tot'>0</span></div>");

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        console.log(e.jqXHR);
        let strmessage = e.jqXHR.responseJSON.message
        if (strmessage === "") strmessage = "Please contact your vendor"
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

    $('#dt_validate_by_ho').on('change', 'tbody td .dt-checkboxes', function () {
        let rows = dt_validate_by_ho.row(this.closest('tr')).data();
        let totalClaim = rows.totalClaim;
        if (this.checked) {
            arrChecked.push({
                id: rows.id,
                totalClaim: parseFloat(rows.totalClaim)
            });
            sumAmount += parseFloat(totalClaim);
        } else {
            sumAmount -= parseFloat(totalClaim);
            let index = arrChecked.findIndex(p => p.id === rows.id);
            if (index > -1) {
                arrChecked.splice(index, 1);
            }
        }
        $('#tot').html(formatMoney(sumAmount.toString(), 0));
        $('#count').html(arrChecked.length);
    });
});

$('#dt_validate_by_ho_search').on('keyup', function() {
    dt_validate_by_ho.search(this.value).draw();
});

$('#btn_submit').on('click', function () {
    let e = document.querySelector("#btn_submit");
    let dataRowsChecked = [];
    $.each(arrChecked, function (index, value) {
        dataRowsChecked.push({
            dnid: value.id,
        });
    });
    if (dataRowsChecked.length > 0) {
        let formData = new FormData();
        formData.append('dnid', JSON.stringify(dataRowsChecked));
        let url = "/dn/validate-by-ho/approved";
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
                        text: result.message,
                        icon: "success",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function () {
                        dt_validate_by_ho.ajax.reload();
                        resetForm();
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
    } else {
        Swal.fire({
            title: swalTitle,
            text: "Please select one or more items",
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
        });
    }
});

$('#btn_download').on('click', function() {
    const current_url = window.location.href;
    const url_object = new URL(current_url);
    const protocol = url_object.protocol;
    const domain = url_object.hostname;
    const port = url_object.port;

    var url;
    if (port == "") {
        url = protocol + "//" + domain + "/" + 'assets/media/templates/Template_DN_Upload_Validation.xlsx';
    } else {
        url = protocol + "//" + domain + ":" + port + "/" + 'assets/media/templates/Template_DN_Upload_Validation.xlsx';
    }
    fetch(url)
        .then(resp => resp.blob())
        .then(blob => {
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = url;
            // the filename you want
            let name_file = 'Template DN Validation.xlsx';
            a.download = name_file;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
        });
});

$('#btn-upload').on('click', function() {
    $('#form-upload').toggle();
});

$('#btn_upload').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#btn_upload");
            let formData = new FormData($('#form_upload_validate')[0]);
            let url = '/dn/validate-by-ho/upload-xls';
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
                            dt_validate_by_ho.clear().draw();
                            let el_header = $('#dt_validate_by_ho_wrapper #dt-checkbox-header');
                            let data = [];
                            if (el_header[0].checked == false) {
                                if (result.data) {
                                    if (result.data.length > 0) {
                                        result.data.forEach(function(el, index) {
                                            if (el.checkFlag == true) {
                                                checkFlag.push(
                                                    el.id
                                                );
                                                data.push({
                                                    id: el.id,
                                                    refId: el.refId,
                                                    promoRefId: el.promoRefId,
                                                    activityDesc: el.activityDesc,
                                                    dpp: el.dpp,
                                                    lastStatus: el.lastStatus,
                                                    isDNPromo: el.isDNPromo,
                                                    totalClaim: el.totalClaim,
                                                    docCount: 1

                                                });
                                            } else {
                                                data.push({
                                                    id: el.id,
                                                    refId: el.refId,
                                                    promoRefId: el.promoRefId,
                                                    activityDesc: el.activityDesc,
                                                    dpp: el.dpp,
                                                    lastStatus: el.lastStatus,
                                                    isDNPromo: el.isDNPromo,
                                                    totalClaim: el.totalClaim,
                                                    docCount: 0
                                                });
                                            }
                                        });
                                    }
                                }
                                dt_validate_by_ho.rows.add(data).draw();
                            }
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

const resetForm = () => {
    arrChecked = [];
    sumAmount = 0;
    if (arrChecked.length > 0) {
        for (let i = 0; i < arrChecked.length; i++) {
            sumAmount += parseFloat(arrChecked[i].totalClaim);
        }
    }
    $('#tot').html(formatMoney(sumAmount.toString(), 0));
    $('#count').html(arrChecked.length);
}
