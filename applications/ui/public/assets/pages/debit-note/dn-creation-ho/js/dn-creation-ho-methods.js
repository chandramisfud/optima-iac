'use strict';

var dt_dn_creation;
var swalTitle = "Debit Note";
heightContainer = 280;
let dialerObject;

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
    dialerObject = new KTDialer(dialerElement, {
        step: 1,
    });

    Promise.all([getListSubAccount()]).then(async function () {
        $('#filter_period').val(new Date().getFullYear());
        dialerObject.setValue(new Date().getFullYear());
    });

    dt_dn_creation = $('#dt_dn_creation').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: "/dn/creation/list/paginate/filter?period=" +  $('#filter_period').val() + "&subAccountId=" + $('#filter_subaccount').val(),
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
                    if (full.isCancel) {
                        return '<div class="me-0">' +
                            '<a class="btn show menu-dropdown disabled"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">' +
                            '<i class="la la-cog fs-3 text-optima text-hover-optima"></i>' +
                            '</a>' +
                            '</div>';
                    } else {
                        if (full.lastStatus === 'send_to_dist_ho' || full.lastStatus === 'created') {
                            return '<div class="me-0">' +
                                '<a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">' +
                                '<i class="la la-cog fs-3 text-optima text-hover-optima"></i>' +
                                '</a>' +
                                '<div class="menu menu-sub menu-sub-dropdown w-180px w-md-180px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">' +
                                '<a class="dropdown-item text-start edit-record" href="/dn/creation-ho/form?method=update&id=' + data + '"><i class="fa fa-file-alt fs-6 me-2"></i> Edit DN </a>' +
                                '<a class="dropdown-item text-start btn-cancel" href="/dn/creation/dn-cancel?id=' + data + '"><i class="fa fa-trash-alt fs-6 me-2"></i> Cancel DN </a>' +
                                '<a class="dropdown-item text-start print-record" href="/dn/creation-ho/print-pdf?&id=' + data + '" target="_blank"><i class="fa fa-print fs-6 me-2"></i> Print DN</a>' +
                                '<a class="dropdown-item text-start upload-attach" href="/dn/creation-ho/dn-upload-attach?method=upload&id=' + data + '"><i class="fa fa-cloud-upload-alt fs-6 me-2"></i> Upload DN Attach </a>' +
                                '<a class="dropdown-item text-start view-record" href="/dn/creation-ho/dn-display?method=view&id=' + data + '"><i class="la la-opencart fs-6 me-2"></i> DN Display </a>' +
                                '</div>' +
                                '</div>';
                        } else {
                            return '<div class="me-0">' +
                                '<a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">' +
                                '<i class="la la-cog fs-3 text-optima text-hover-optima"></i>' +
                                '</a>' +
                                '<div class="menu menu-sub menu-sub-dropdown w-180px w-md-180px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">' +
                                '<a class="dropdown-item text-start print-record" href="/dn/creation-ho/print-pdf?&id=' + data + '" target="_blank"><i class="fa fa-print fs-6 me-2"></i> Print DN</a>' +
                                '<a class="dropdown-item text-start upload-attach" href="/dn/creation-ho/dn-upload-attach?method=upload&id=' + data + '"><i class="fa fa-cloud-upload-alt fs-6 me-2"></i> Upload DN Attach </a>' +
                                '<a class="dropdown-item text-start view-record" href="/dn/creation-ho/dn-display?method=view&id=' + data + '"><i class="la la-opencart fs-6 me-2"></i> DN Display </a>' +
                                '</div>' +
                                '</div>';
                        }
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
                title: 'DN Description',
                data: 'activityDesc',
                width: 350,
                className: 'align-middle',
            },
            {
                targets: 4,
                title: 'DPP',
                data: 'dpp',
                width: 150,
                className: 'align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 5,
                title: 'Last Status',
                data: 'lastStatus',
                width: 300,
                className: 'align-middle'
            },
            {
                targets: 6,
                title: 'Category',
                data: 'dnCategory',
                width: 100,
                className: 'align-middle',
            },
            {
                targets: 7,
                title: 'TaxLevel',
                data: 'materialNumber',
                width: 250,
                className: 'align-middle',
                render: function (data, type, full, meta) {
                    if (data == null) {
                        return ''
                    } else {
                        return data + ' - ' + full.taxLevel
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

$('#filter_period').on('blur', async function () {
    let valPeriod = parseInt($(this).val() ?? "0");
    if (valPeriod < 2019) {
        $(this).val("2019");
        dialerObject.setValue(2019);
    }
});

$('#dt_dn_creation_search').on('keyup', function () {
    dt_dn_creation.search(this.value).draw();
});

$('#btn_create').on('click', function (){
    checkFormAccess('create_rec', '', '/dn/creation-ho/form', '')
});

$('#btn_export_excel').on('click', function() {
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_subaccount = ($('#filter_subaccount').val()) ?? "";
    let url = "/dn/creation-ho/export-xls?period=" + filter_period + "&subAccountId=" + filter_subaccount;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#dt_dn_creation_view').on('click', function (){
    let btn = document.getElementById('dt_dn_creation_view');
    let filter_period = ($('#filter_period').val()) ?? "";
    let filter_subaccount = ($('#filter_subaccount').val()) ?? "";
    let url = "/dn/creation-ho/list/paginate/filter?period=" + filter_period + "&subAccountId=" + filter_subaccount;
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
            url         : "/dn/creation-ho/list/subaccount",
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
