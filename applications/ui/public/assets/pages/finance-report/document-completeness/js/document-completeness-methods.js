'use strict';

var dt_document_completeness;
var swalTitle = "Document Completeness";
var heightContainer = 280;
var dataFilter, url_datatable;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    if (localStorage.getItem('financeDocCompletenessState')) {
        dataFilter = JSON.parse(localStorage.getItem('financeDocCompletenessState'));

        url_datatable = '/fin-rpt/doc-completeness/list/paginate/filter?entityId=' + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId;
    } else {
        url_datatable = '/fin-rpt/doc-completeness/list/paginate/filter?entityId=' + $('#filter_entity').val() + "&distributorId=" + $('#filter_distributor').val();
    }

    Promise.all([getListEntity()]).then(async function () {
        if (dataFilter) {
            await $('#filter_entity').val(dataFilter.entityId).trigger('change.select2');
            await getListDistributor(dataFilter.entityId);
            $('#filter_distributor').val(dataFilter.distributorId).trigger('change');
        }
    });

    dt_document_completeness = $('#dt_document_completeness').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: url_datatable,
            type: 'get',
        },
        processing: true,
        serverSide: true,
        paging: true,
        searching: true,
        scrollX: true,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'DN Number',
                data: 'refId',
                width: 170,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'Promo ID',
                data: 'promoRefId',
                width: 170,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Acitvity',
                data: 'activityDesc',
                width: 300,
                className: 'align-middle',
            },
            {
                targets: 3,
                title: 'Total Claim',
                data: 'totalClaim',
                width: 170,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatMoney(data, 0, ".", ",");
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 4,
                title: 'Last Status',
                data: 'lastStatus',
                width: 150,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Last Update',
                data: 'lastUpdate',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (!data) {
                        return '';
                    } else {
                        return formatDate(data);
                    }
                }
            },
            {
                targets: 6,
                title: 'TaxLevel',
                data: 'materialNumber',
                width: 300,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data === null || data === "" || full.taxLevel === null) {
                        return ''
                    } else {
                        return data + ' - ' + full.taxLevel
                    }
                }
            },
            {
                targets: 7,
                title: 'Rejection Remarks by Finance',
                data: 'notes',
                width: 300,
                className: 'align-middle',
            },
            {
                targets: 8,
                title: 'SP No',
                data: 'sp_principal',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 9,
                title: 'Remark by Sales',
                data: 'remarkSales',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 10,
                title: 'Sales Validation Status',
                data: 'salesValidationStatus',
                width: 200,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 11,
                title: 'FP Number',
                data: 'fpNumber',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 12,
                title: 'FP Date',
                data: 'fpDate',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
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

});

$('#dt_document_completeness_search').on('keyup', function () {
    dt_document_completeness.search(this.value).draw();
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

$('#dt_document_completeness_view').on('click', function (){
    let btn = document.getElementById('dt_document_completeness_view');
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/fin-rpt/doc-completeness/list/paginate/filter?entityId=" + filter_entity + "&distributorId=" + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_document_completeness.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

        let data_filter = {
            entityId: ($('#filter_entity').val() ?? ""),
            distributorId: ($('#filter_distributor').val() ?? "")
        };

        localStorage.setItem('financeDocCompletenessState', JSON.stringify(data_filter));
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_excel').on('click', function() {
    let text_entity = "";
    let data = $('#filter_entity').select2('data');
    (data[0].id !== "") ? text_entity = data[0].longDesc : text_entity ='ALL';
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/fin-rpt/doc-completeness/export-xls?entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&entity=" + text_entity;
    console.log(url)

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/doc-completeness/list/entity",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].shortDesc + " - " + result.data[j].longDesc,
                        longDesc: result.data[j].longDesc
                    });
                }
                $('#filter_entity').select2({
                    placeholder: "Select an Entity",
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

const getListDistributor = (entityId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/doc-completeness/list/distributor/entity-id",
            type        : "GET",
            dataType    : 'json',
            data        : {entityId: entityId},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#filter_distributor').select2({
                    placeholder: "Select a Distributor",
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
