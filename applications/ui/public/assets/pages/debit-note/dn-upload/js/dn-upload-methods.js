'use strict';

var dt_dn_creation, distributorId;
var swalTitle = "Debit Note";
heightContainer = 280;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    $('#filter_closing_date').flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });

    KTDialer.createInstances();
    let dialerElement = document.querySelector("#dialer_period");
    let dialerObject = new KTDialer(dialerElement, {
        min: 2015,
        step: 1,
    });

    Promise.all([getListSubAccount(), getDistributorId()]).then(async function () {
        $('#filter_period').val(new Date().getFullYear());
        dialerObject.setValue(new Date().getFullYear());
    });

    dt_dn_creation = $('#dt_dn_creation').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'desc']],
        ajax: {
            url: "/dn/upload/list/paginate/filter?period=" +  $('#filter_period').val() + "&subAccountId=" + $('#filter_subaccount').val(),
            type: 'get',
        },
        processing: true,
        serverSide: true,
        paging: true,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 20,
                title: '',
                orderable: false,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    if (full.lastStatus === 'send_to_dist_ho' || full.lastStatus === 'created') {
                        return '<div class="me-0">' +
                            '<a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">' +
                            '<i class="la la-cog fs-3 text-optima text-hover-optima"></i>' +
                            '</a>' +
                            '<div class="menu menu-sub menu-sub-dropdown w-auto w-md-180px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">' +
                            '<a class="dropdown-item text-start edit-record" href="/dn/upload/form?method=update&id=' + data + '"><i class="fa fa-file-alt fs-6 me-2"></i> Edit DN </a>' +
                            '<a class="dropdown-item text-start btn-cancel" href="/dn/upload/dn-cancel?id=' + data + '"><i class="fa fa-trash-alt fs-6 me-2"></i> Cancel DN </a>' +
                            '<a class="dropdown-item text-start print-record" href="/dn/upload/print-pdf?&id=' + data + '" target="_blank"><i class="fa fa-print fs-6 me-2"></i> Print DN</a>' +
                            '<a class="dropdown-item text-start upload-attach" href="/dn/upload/dn-upload-attach?method=upload&id=' + data + '"><i class="fa fa-cloud-upload-alt fs-6 me-2"></i> Upload DN Attach </a>' +
                            '</div>' +
                        '</div>';
                    } else {
                        return '<div class="me-0">' +
                            '<a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">' +
                            '<i class="la la-cog fs-3 text-optima text-hover-optima"></i>' +
                            '</a>' +
                            '<div class="menu menu-sub menu-sub-dropdown w-auto w-md-180px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">' +
                            '<a class="dropdown-item text-start print-record" href="/dn/upload/print-pdf?&id=' + data + '" target="_blank"><i class="fa fa-print fs-6 me-2"></i> Print DN</a>' +
                            '<a class="dropdown-item text-start upload-attach" href="/dn/upload/dn-upload-attach?method=upload&id=' + data + '"><i class="fa fa-cloud-upload-alt fs-6 me-2"></i> Upload DN Attach </a>' +
                            '</div>' +
                        '</div>';
                    }

                }
            },
            {
                targets: 1,
                data: 'refId',
                width: 170,
                title: 'DN Number',
                className: 'align-middle',
            },
            {
                targets: 2,
                data: 'promoRefId',
                width: 170,
                title: 'Promo ID',
                className: 'align-middle',
            },
            {
                targets: 3,
                title: 'Internal Doc. No',
                data: 'intDocNo',
                width: 170,
                className: 'align-middle',
            },
            {
                targets: 4,
                title: 'DN Description',
                data: 'activityDesc',
                width: 350,
                className: 'align-middle',
            },
            {
                targets: 5,
                title: 'Total Claim',
                data: 'totalClaim',
                width: 150,
                className: 'align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 6,
                title: 'Last Status',
                data: 'lastStatus',
                width: 300,
                className: 'align-middle'
            },
            {
                targets: 7,
                title: 'Last Update',
                data: 'lastUpdate',
                width: 100,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
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

$('#dt_dn_creation_search').on('keyup', function () {
    dt_dn_creation.search(this.value).draw();
});

$('#btn-upload').on('click', function() {
    window.location.href = "/dn/upload/upload-form";
});

$('#dt_dn_creation_view').on('click', function (){
    let btn = document.getElementById('dt_dn_creation_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_subaccount = ($('#filter_subaccount').val()) ?? "";
    let url = "/dn/upload/list/paginate/filter?period=" + filter_period + "&subAccountId=" + filter_subaccount;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_dn_creation.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

const getListSubAccount = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dn/upload/list/subaccount",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#filter_subaccount').select2({
                    placeholder: "Select a Sub Account",
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

const getDistributorId = () => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/dn/upload/get-data/distributor",
            type: "GET",
            dataType: 'json',
            async: true,
            success: async function (result) {
                distributorId = result.data;
            },
            complete: function(result) {
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                if (jqXHR.status == 403) {
                    Swal.fire({
                        title: swalTitle,
                        text: jqXHR.responseJSON.message,
                        icon: "warning",
                        confirmButtonText: "OK",
                        allowOutsideClick: false,
                        allowEscapeKey: false,
                        customClass: { confirmButton: "btn btn-optima" }
                    }).then(function () {
                        window.location.href = '/login-page';
                    });
                }
                return reject(jqXHR.responseText);
            },
        });
    }).catch((e) => {
        console.log(e);
    });
}

$('#btn_download').on('click', function() {
    let url = file_host + '/assets/media/templates/Template_DN' + distributorId + '.xlsx';
    fetch(url).then((resp) => {
        if (resp.ok) {
            resp.blob().then(blob => {
                const url_blob = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.style.display = 'none';
                a.href = url_blob;
                a.download = 'Template DN';
                document.body.appendChild(a);
                a.click();
                window.URL.revokeObjectURL(url_blob);
            })
                .catch(e => {
                    console.log(e);
                    Swal.fire({
                        text: "Download template failed",
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
                });
        } else {
            Swal.fire({
                title: "Download Template",
                text: "Template not found",
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
    });
});
