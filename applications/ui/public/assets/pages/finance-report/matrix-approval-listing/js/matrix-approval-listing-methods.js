'use strict';

let dt_matrix_approval_listing;
let swalTitle = "Matrix Approval Listing";
heightContainer = 280;
let dataFilter, url_datatable;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    if (localStorage.getItem('financeMatrixApprovalListingReportState')) {
        dataFilter = JSON.parse(localStorage.getItem('financeMatrixApprovalListingReportState'));

        url_datatable = "/fin-rpt/matrix-approval-listing/list/paginate/filter?categoryId=" + (dataFilter.categoryId ?? "0") + "&entityId=" + dataFilter.entityId + "&distributorId=" + dataFilter.distributorId;
    } else {
        url_datatable = "/fin-rpt/matrix-approval-listing/list/paginate/filter?categoryId=" + $('#filter_category').val() + "&entityId=" + $('#filter_entity').val() + "&distributorId=" + $('#filter_distributor').val();
    }


    Promise.all([getListCategory(), getListEntity()]).then(async function () {
        if (dataFilter) {
            if(dataFilter.categoryId) await $('#filter_category').val(dataFilter.categoryId).trigger('change.select2');
            await $('#filter_entity').val(dataFilter.entityId).trigger('change.select2');
            await getListDistributor(dataFilter.entityId);
            $('#filter_distributor').val(dataFilter.distributorId).trigger('change');
        }
    });

    dt_matrix_approval_listing = $('#dt_matrix_approval_listing').DataTable({
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
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'entity',
                width: 250,
                title: 'Entity',
                className: 'align-middle',
            },
            {
                targets: 1,
                title: 'Distributor',
                data: 'distributor',
                width: 250,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Category',
                data: 'categoryLongDesc',
                width: 150,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data) {
                        return data;
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 3,
                title: 'Initiator',
                data: 'initiator',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Channel',
                data: 'channel',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Sub Channel',
                data: 'subChannel',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Sub Activity Type',
                data: 'subActivityType',
                width: 120,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Min. Investment',
                data: 'minInvestment',
                width: 120,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 8,
                title: 'Max. Investment',
                data: 'maxInvestment',
                width: 120,
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    return formatMoney(data, 0,".", ",");
                }
            },
            {
                targets: 9,
                title: 'Matrix Approver',
                data: 'matrixApprover',
                width: 450,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 10,
                title: 'Last Update',
                data: 'modifiedOn',
                width: 100,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (formatDateOptima(data) === '01-01-1970') {
                        return '';
                    } else {
                        return formatDateOptima(data);
                    }
                }
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

$('#dt_matrix_approval_listing_search').on('keyup', function () {
    dt_matrix_approval_listing.search(this.value).draw();
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getListDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

$('#dt_matrix_approval_listing_view').on('click', function (){
    let btn = document.getElementById('dt_matrix_approval_listing_view');
    let filter_category = ($('#filter_category').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/fin-rpt/matrix-approval-listing/list/paginate/filter?categoryId=" + filter_category + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor;
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;
    dt_matrix_approval_listing.ajax.url(url).load(function () {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

        let data_filter = {
            categoryId: ($('#filter_category').val() ?? ""),
            entityId: ($('#filter_entity').val() ?? ""),
            distributorId: ($('#filter_distributor').val() ?? "")
        };

        localStorage.setItem('financeMatrixApprovalListingReportState', JSON.stringify(data_filter));
    }).on('xhr.dt', function ( e, settings, json, xhr ) {
        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;
    });
});

$('#btn_export_current').on('click', function() {
    let text_entity = "";
    let data_entity = $('#filter_entity').select2('data');
    if (data_entity.length > 0) {
        (data_entity[0].id !== "") ? text_entity = data_entity[0].longDesc : text_entity ='ALL';
    } else {
        text_entity ='ALL'
    }

    let filter_category = ($('#filter_category').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/fin-rpt/matrix-approval-listing/export-xls?categoryId=" + filter_category + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&entity=" + text_entity;

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_export_historical').on('click', function() {
    let text_entity = "";
    let data_entity = $('#filter_entity').select2('data');
    if (data_entity.length > 0) {
        (data_entity[0].id !== "") ? text_entity = data_entity[0].longDesc : text_entity ='ALL';
    } else {
        text_entity ='ALL'
    }

    let filter_category = ($('#filter_category').val()) ?? "";
    let filter_entity = ($('#filter_entity').val()) ?? "";
    let filter_distributor = ($('#filter_distributor').val()) ?? "";
    let url = "/fin-rpt/matrix-approval-listing/export-xls/historical?categoryId=" + filter_category + "&entityId=" + filter_entity + "&distributorId=" + filter_distributor + "&entity=" + text_entity;

    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

const getListCategory = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/matrix-approval-listing/list/category",
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
                $('#filter_category').select2({
                    placeholder: "Select a Category",
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

const getListEntity = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/matrix-approval-listing/list/entity",
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
            url         : "/fin-rpt/matrix-approval-listing/list/distributor/entity-id",
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
