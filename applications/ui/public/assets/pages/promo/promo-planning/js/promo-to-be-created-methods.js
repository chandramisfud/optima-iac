'use strict';

let dt_promo_to_be_created, elDtPromoToBeCreated = $('#dt_promo_to_be_created');
let swalTitle = "Promo To Be Created";
heightContainer = 320;
let dataFilter, url_datatable;

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    $('#filter_activity_start, #filter_activity_end').flatpickr({
        altFormat: "Y-m-d",
        dateFormat: "Y-m-d",
        disableMobile: "true",
    });

    if (localStorage.getItem('promoTobeCreatedState')) {
        dataFilter = JSON.parse(localStorage.getItem('promoTobeCreatedState'));

        url_datatable = '/promo/planning/list/to-be-created';
    } else {
        url_datatable = '/promo/planning/list/to-be-created';
    }

    dt_promo_to_be_created = elDtPromoToBeCreated.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>" +
            "<'row '<'col-sm-1'l><'col-sm-4'i><'col-sm-7'p>>",
        order: [[1, 'asc']],
        ajax: {
            url: url_datatable,
            type: 'get',
        },
        saveState: true,
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
                data: 'promoPlanId',
                width: 20,
                orderable: false,
                title: '',
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full) {
                    let cancel, createPromo, popMenu
                    if (full.lastStatus) {
                        if (full.lastStatus.substring(0, 9) === 'Cancelled') {
                            cancel = ''
                        } else {
                            cancel = '<a class="dropdown-item text-start cancel-record" href="#"><i class="fa fa-undo fs-6"></i> Cancel Planning</a>'
                        }
                    }
                    if (full.tsCode !== "" && full.tsCode != null) {
                        createPromo = '<a class="dropdown-item text-start create-promo" href="/promo/creation/form?method=createPromo&promoPlanId=' + full.promoPlanId + '"><i class="fa fa-paper-plane fs-6"></i> Create Promo</a>';
                    } else {
                        createPromo = '<a class="dropdown-item text-start edit-record" href="/promo/planning/form?method=update&promoPlanId=' + full.promoPlanId + '"><i class="fa fa-edit fs-6"></i> Edit Data</a>';
                    }

                    if (full.promoId !== 0 || full.lastStatus.substring(0, 9) === 'Cancelled') {
                        popMenu = '\
                            <div class="me-0">\
                                <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                    <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                                </a>\
                                <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                    <a class="dropdown-item text-start duplicate-record" href="/promo/planning/form?method=duplicate&promoPlanId=' + full.promoPlanId + '"><i class="fa fa-file-archive fs-6"></i> Duplicate Data</a>\
                                </div>\
                            </div>\
                        ';
                    } else {
                        popMenu = '\
                            <div class="me-0">\
                                <a class="btn show menu-dropdown"  data-kt-menu-trigger="click" data-kt-menu-placement="bottom-start">\
                                    <i class="la la-cog fs-3 text-optima text-hover-optima"></i>\
                                </a>\
                                <div class="menu menu-sub menu-sub-dropdown w-auto shadow p-3 mb-5 bg-body rounded" data-kt-menu="true">\
                                    <a class="dropdown-item text-start duplicate-record" href="/promo/planning/form?method=duplicate&promoPlanId=' + full.promoPlanId + '"><i class="fa fa-copy fs-6"></i> Duplicate Data</a>\
                                    ' + cancel + '\
                                    ' + createPromo + '\
                                </div>\
                            </div>\
                        ';
                    }
                    return popMenu;
                }
            },
            {
                targets: 1,
                data: 'refId',
                width: 120,
                orderable: false,
                title: 'Planning ID',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 2,
                title: 'TSCode',
                data: 'tsCode',
                orderable: false,
                width: 100,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 3,
                title: 'Entity',
                data: 'entityShortDesc',
                width: 50,
                orderable: false,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 4,
                title: 'Distributor',
                data: 'distributorShortDesc',
                width: 70,
                orderable: false,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 5,
                title: 'Sub Account',
                data: 'subAccountDesc',
                width: 150,
                orderable: false,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 6,
                title: 'Brand',
                data: 'brandDesc',
                width: 500,
                orderable: false,
                className: 'align-middle',
                createdCell: function (cell, data) {
                    let $cell = $(cell);

                    if (data) {
                        $(cell).contents().wrapAll('<div class="content"></div>');
                        let $content = $cell.find(".content");
                        if (data.length > 45)
                            $(cell).append($('<button class="btn btn-clean-more">Read more</button>'));
                        let $btn = $(cell).find("button");

                        $content.css({
                            "padding": "0",
                            "height": "20px",
                            "width": "300px",
                            "overflow": "hidden"
                        })
                        $cell.data("isLess", true);

                        $btn.click(function () {
                            let isLess = $cell.data("isLess");
                            $content.css("height", isLess ? "auto" : "20px")
                            $(this).text(isLess ? "Read less" : "Read more")
                            $cell.data("isLess", !isLess)
                        })
                    }
                }
            },
            {
                targets: 7,
                title: 'Mechanism',
                data: 'mechanism',
                width: 250,
                orderable: false,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 8,
                title: 'Promo Start',
                data: 'startPromo',
                width: 80,
                orderable: false,
                className: 'align-middle text-nowrap',
                render: function (data) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 9,
                title: 'Promo End',
                data: 'endPromo',
                width: 80,
                orderable: false,
                className: 'align-middle text-nowrap',
                render: function (data) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 10,
                title: 'Activity Description',
                data: 'activityDesc',
                width: 250,
                orderable: false,
                className: 'align-middle text-nowrap',
            },
            {
                targets: 11,
                title: 'Investment',
                data: 'investment',
                width: 100,
                orderable: false,
                className: 'align-middle text-end',
                render: function (data) {
                    return formatMoney(data, 0, ".", ",");
                }
            },
            {
                targets: 12,
                title: 'Last Status',
                data: 'lastStatus',
                orderable: false,
                className: 'align-middle text-nowrap'
            },
            {
                targets: 13,
                title: 'Promo ID',
                data: 'promoRefId',
                width: 120,
                orderable: false,
                className: 'align-middle text-nowrap'
            },
            {
                targets: 14,
                title: 'Cancel Notes',
                data: 'cancelNotes',
                width: 200,
                orderable: false,
                className: 'align-middle text-nowrap'
            },
            {
                targets: 15,
                title: 'Initiator Notes',
                data: 'initiator_notes',
                width: 200,
                orderable: false,
                className: 'align-middle text-nowrap'
            },
            {
                targets: 16,
                title: 'TSCode On',
                data: 'tsCodeOn',
                width: 120,
                orderable: false,
                className: 'align-middle text-nowrap',
                render: function (data) {
                    if (data) {
                        return formatDateTime(data);
                    } else {
                        return "";
                    }
                }
            },
            {
                targets: 17,
                title: 'TSCode By',
                data: 'tsCodeBy',
                width: 80,
                orderable: false,
                className: 'align-middle text-nowrap'
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function () {
            KTMenu.createInstances();
        },
    });

    $.fn.dataTable.ext.errMode = function (e) {
        let strMessage = e.jqXHR.responseJSON.message
        if (strMessage === "") strMessage = "Please contact your vendor"
        Swal.fire({
            text: strMessage,
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-optima"},
            allowOutsideClick: false,
            allowEscapeKey: false,
        });
    };

    elDtPromoToBeCreated.on('click', '.cancel-record', function () {
        let tr = this.closest("tr");
        let trdata = dt_promo_to_be_created.row(tr).data();
        let promoPlanningId = parseInt(trdata.promoPlanId);

        Swal.fire({
            title: 'Are you sure to cancel ' + trdata.refId + ' ?',
            text: "You won't be able to revert this",
            icon: "warning",
            allowOutsideClick: false,
            allowEscapeKey: false,
            showCancelButton: true,
            confirmButtonText: 'Submit',
            cancelButtonText: 'Cancel',
            customClass: {
                confirmButton: "btn btn-optima",
                cancelButton: "btn btn-optima",
            },
            html:
                '<select class="form-select form-select-sm mb-2" data-control="select2" name="reason" id="reason" data-placeholder="Select a Reason" data-allow-clear="true">' +
                '   <option value="Brand plan amendment">Brand plan amendment</option>\n' +
                '   <option value="Retailers operational issue">Retailers operational issue</option>\n' +
                '   <option value="Danones management decision">Danones management decision</option>\n' +
                '   <option value="Miscalculation or human error">Miscalculation or human error</option>\n' +
                '   <option value="Others">Others</option>\n' +
                '</select>\n' +
                '<input type="text" id="reason-text" class="form-control form-control-sm d-none">',
            didOpen: () => {
                $('#reason').on('change', function () {
                    if ($('#reason').val() === "Others") {
                        $('#reason-text').removeClass('d-none');
                    } else {
                        $('#reason-text').addClass('d-none');
                    }
                });
            },
        }).then(function (result) {
            if (result.value) {
                let reason;
                let elReason = $('#reason');
                if (elReason.val() === "Others") {
                    reason = $('#reason-text').val();
                } else {
                    reason = elReason.val();
                }
                $.ajax({
                    url: '/promo/planning/cancel',
                    type: "POST",
                    data: {promoPlanningId: promoPlanningId, reason: reason},
                    dataType: "JSON",
                    async: false,
                    success: function (result) {
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
                                window.location.href = '/promo/planning/to-be-created';
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
                    error: function (jqXHR) {
                        console.log(jqXHR);
                    }
                });
            }
        });
    });
});

$('#dt_promo_to_be_created_search').on('keyup', function () {
    dt_promo_to_be_created.search(this.value).draw();
});

$('#btn_create').on('click', function () {
    checkFormAccess('create_rec', '', '/promo/planning/form', '')
});

const checkFormAccessManual = (menuId, trxAction, id, url, popMsg, confirmButtonText = "Yes, delete it", type = "delete") => {
    $.ajax({
        url: "/promo/planning/check-form-access",
        type: "GET",
        data: {menuid: menuId, access_name: trxAction},
        dataType: "JSON",
        async: true,
        success: function (result) {
            if (!result.error) {
                switch(trxAction) {
                    case "create_rec":
                        window.location.href = url;
                        break;
                    case "update_rec":
                        window.location.href = url;
                        break;
                    case "delete_rec":
                        Swal.fire({
                            title: swalTitle,
                            text: popMsg,
                            icon: 'warning',
                            showCancelButton: true,
                            confirmButtonColor: '#3085d6',
                            cancelButtonColor: '#AAAAAA',
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            cancelButtonText: 'No, cancel',
                            confirmButtonText: confirmButtonText,
                            reverseButtons: true
                        }).then((result) => {
                            if (result.isConfirmed) {
                                if(type === "activate"){
                                    fActivateRecord(id);
                                } else if (type === 'deactivate'){
                                    fDeactivateRecord(id);
                                } else {
                                    fDeleteRecord(id);
                                }
                            }
                        })
                        break;
                    default:
                    // code block
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
        error: function (jqXHR) {
            if (jqXHR.status === 403) {
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
            console.log(jqXHR);
        },
    });
}
