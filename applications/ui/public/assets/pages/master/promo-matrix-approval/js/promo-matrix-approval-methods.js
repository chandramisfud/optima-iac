'use strict';

var dt_matrix_approval;
var swalTitle = "Matrix Promo Approval";
heightContainer = 280;

$(document).ready(function () {
    getEntity();

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    KTDialer.createInstances();

    dt_matrix_approval = $('#dt_matrix_approval').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: '/master/matrix/promoapproval/list',
            type: 'get',
        },
        processing: true,
        serverSide: false,
        paging: true,
        searching: true,
        scrollX: true,
        lengthMenu: [[10, 20, 50, 100, -1], [10, 20, 50, 100, "All"]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'id',
                width: 20,
                orderable: false,
                title: '',
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    return '\
                        <div class="me-0">\
                            <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                            </a>\
                            <div class="menu menu-sub menu-sub-dropdown w-125px w-md-125px shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                <a class="dropdown-item text-start edit-record" href="/master/matrix/promoapproval/form?method=update&matrixapprovalid=' + data + '"><i class="fa fa-edit fs-6"></i> Edit Data</a>\
                            </div>\
                        </div>\
                    ';
                }
            },
            {
                targets: 1,
                title: 'Entity',
                data: 'entity',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'Distributor',
                data: 'distributor',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Category',
                data: 'categoryLongDesc',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Sub Activity Type',
                data: 'subActivityType',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 5,
                title: 'Channel',
                data: 'channel',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 6,
                title: 'Sub Channel',
                data: 'subChannel',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 7,
                title: 'Min. Investment',
                data: 'minInvestment',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatMoney(data, 0,".", ",");
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 8,
                title: 'Max. Investment',
                data: 'maxInvestment',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatMoney(data, 0,".", ",");
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 9,
                title: 'Matrix Approver',
                data: 'matrixApprover',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {
            KTMenu.createInstances();
        },
    });

    $('#dt_matrix_approval_search').on('keyup', function () {
        dt_matrix_approval.search(this.value).draw();
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

$('#filter_collapsible').on('hidden.bs.collapse', function () {
    $('div.dataTables_scrollBody').height( "63vh" );
})

$('#filter_collapsible').on('shown.bs.collapse', function () {
    $('div.dataTables_scrollBody').height( "55vh" );
})

$('#btn_create').on('click', function() {
    checkFormAccess('create_rec', '', '/master/matrix/promoapproval/form', '')
});

$('#btn_upload_form').on('click', function () {
    checkFormAccess('create_rec', '', '/master/matrix/promoapproval/upload-form', '');
});

$('#filter_entity').on('change', async function () {
    blockUI.block();
    $('#filter_distributor').empty();
    if ($(this).val() !== "") await getDistributor($(this).val());
    blockUI.release();
    $('#filter_distributor').val('').trigger('change');
});

$('#dt_matrix_approval_view').on('click', function (){
    let e = document.getElementById('dt_matrix_approval_view');
    let entityId = ($('#filter_entity').val()) ?? "";
    let distributorId = ($('#filter_distributor').val()) ?? "";
    let periode = ($('#filter_period').val()) ?? "";
    let url = "/master/matrix/promoapproval/list?entity=" + entityId + "&distributor=" + distributorId + "&periode=" + periode ;
    e.setAttribute("data-kt-indicator", "on");
    e.disabled = !0;
    dt_matrix_approval.ajax.url(url).load(function () {
        e.setAttribute("data-kt-indicator", "off");
        e.disabled = !1;
    });
});

const getEntity = () => {
    $.ajax({
        url         : "/master/matrix/promoapproval/get-list/entity",
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

        },
        error: function (jqXHR, textStatus, errorThrown)
        {
            console.log(jqXHR.responseText);
        }
    });
}

const getDistributor = (entityid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/master/matrix/promoapproval/get-data/distributor/entity-id",
            type        : "GET",
            dataType    : 'json',
            data        : {PrincipalId: entityid},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].distributorId,
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
        return reject(e);
    });
}

$('#btn_export_current').on('click', function() {
    let entity = $('#filter_entity').val();
    let entityText = $("#filter_entity option:selected").text()
    let distributor = $('#filter_distributor').val();

    let url = "/master/matrix/promoapproval/export-xls?entity=" + entity + "&entityText=" + entityText + "&distributor=" + distributor;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});

$('#btn_export_historical').on('click', function() {
    let entity = $('#filter_entity').val();
    let entityText = $("#filter_entity option:selected").text()
    let distributor = $('#filter_distributor').val();

    let url = "/master/matrix/promoapproval/export-xls/historical?entity=" + entity + "&entityText=" + entityText + "&distributor=" + distributor;
    let a = document.createElement("a");
    a.href = url;
    let evt = document.createEvent("MouseEvents");
    //the tenth parameter of initMouseEvent sets ctrl key
    evt.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0,
        true, false, false, false, 0, null);
    a.dispatchEvent(evt);
});
