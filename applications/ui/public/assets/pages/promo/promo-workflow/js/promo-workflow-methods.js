'use strict';

let dt_mechanism, dt_workflow_history, dt_change_history, dt_dn_list;
let swalTitle = "Promo ID Workflow";
let promoId, refId;

let elDtSKU = $('#dt_sku');
let elDtMechanism = $('#dt_mechanism_revamp');
let dt_mechanism_revamp, dt_sku;

let categoryDesc = "Retailer Cost";
let yearPromo, statusMechanismList;

const targetDataPromoRevamp = document.querySelector(".card_data_promo_revamp");
const blockUIDataPromoRevamp = new KTBlockUI(targetDataPromoRevamp, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

const targetDataPromo = document.querySelector(".card_data_promo");
const blockUIDataPromo = new KTBlockUI(targetDataPromo, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

const targetDataPromoDC = document.querySelector(".card_data_promo_dc");
const blockUIDataPromoDC = new KTBlockUI(targetDataPromoDC, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

const targetWorkflowHistory = document.querySelector(".card_workflow_history");
const blockUIWorkflowHistory = new KTBlockUI(targetWorkflowHistory, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

const targetChangeHistory = document.querySelector(".card_change_history");
const blockUIChangeHistory = new KTBlockUI(targetChangeHistory, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

const targetDNList = document.querySelector(".card_dn_list");
const blockUIDNList = new KTBlockUI(targetDNList, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    let year = new Date().getFullYear();
    if (year < '2025') {
        $('#card_form_rc').removeClass('d-none');
    } else {
        $('#card_form_rc_new').removeClass('d-none');
    }

    dt_mechanism_revamp = elDtMechanism.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: false,
        scrollY: "40vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: 'No',
                targets: 0,
                width: 10,
                data: 'no',
                orderable: false,
                className: 'text-nowrap text-center align-top',
                render: function (data) {
                    return formatMoney(data,0);
                }
            },
            {
                title: 'Mechanism',
                targets: 1,
                data: 'mechanism',
                className: 'align-top',
            },
            {
                title: 'Notes',
                targets: 2,
                data: 'notes',
                className: 'align-top',
            },
            {
                title: 'Cost',
                width: 100,
                targets: 3,
                data: 'cost',
                className: 'text-nowrap align-top text-end pe-1',
                render: function (data) {
                    return formatMoney(data,0);
                }
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
        footerCallback: function (row, data) {

        }
    });

    dt_sku = elDtSKU.DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: false,
        paging: false,
        searching: true,
        scrollX: true,
        scrollY: "27vh",
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'SKU',
                data: 'skuDesc',
                className: 'text-nowrap align-middle cursor-pointer',
            },
        ],
        initComplete: function (settings, json) {

        },
        drawCallback: function (settings, json) {

        },
    });

    $('#dt_sku_search').on('keyup', function () {
        dt_sku.search(this.value, false, false).draw();
    });

    dt_mechanism = $('#dt_mechanism').DataTable({
        dom:
            "<'row'<'col-sm-12'Rtr>>",
        processing: false,
        serverSide: false,
        paging: false,
        ordering: false,
        searching: true,
        scrollX: true,
        scrollY: '20vh',
        autoWidth: false,
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'mechanismId',
                title: 'No',
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return meta.row + 1;
                }
            },
            {
                targets: 1,
                title: 'Mechanism',
                data: 'mechanism',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 2,
                title: 'Notes',
                data: 'notes',
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    dt_workflow_history = $('#dt_workflow_history').DataTable({
        dom:
            "<'row'<'col-sm-12'Rtr>>",
        processing: false,
        serverSide: false,
        paging: false,
        ordering: false,
        searching: true,
        scrollX: true,
        scrollY: '40vh',
        autoWidth: false,
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'date',
                title: 'Date',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'User ID',
                data: 'userid',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 2,
                title: 'Login Email',
                data: 'loginemail',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Status',
                data: 'status',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 4,
                title: 'Investment',
                data: 'investment',
                className: 'text-nowrap align-middle text-end',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatMoney(data, 0,".", ",");
                    } else {
                        return '0';
                    }
                }
            },
            {
                targets: 5,
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

    dt_change_history = $('#dt_change_history').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        processing: false,
        serverSide: false,
        paging: false,
        ordering: false,
        searching: true,
        scrollX: true,
        scrollY: '40vh',
        autoWidth: false,
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                data: 'date',
                title: 'Date',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'User ID',
                data: 'userid',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 2,
                title: 'Login Email',
                data: 'loginemail',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Field',
                data: 'field',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 4,
                title: 'Old Value',
                data: 'oldvalue',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 5,
                title: 'New Value',
                data: 'newvalue',
                className: 'align-middle text-nowrap',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    dt_dn_list = $('#dt_dn_list').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        processing: false,
        serverSide: false,
        paging: false,
        ordering: false,
        searching: true,
        scrollX: true,
        scrollY: '40vh',
        autoWidth: false,
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'DN Number',
                data: 'refid',
                className: 'text-nowrap align-middle',
            },
            {
                targets: 1,
                title: 'Last Status',
                data: 'laststatus',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 2,
                title: 'VAT Expired',
                data: 'vatexpired',
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    return (data) ? 'Y' : 'N';
                }
            },
            {
                targets: 3,
                title: 'FP Number',
                data: 'fpnumber',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 4,
                title: 'FP Date',
                data: 'fpdate',
                className: 'align-middle text-nowrap',
                render: function (data, type, full, meta) {
                    if (data) {
                        return formatDate(data);
                    } else {
                        return '';
                    }
                }
            },
            {
                targets: 5,
                title: 'Tax Level',
                data: 'taxlevel',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 6,
                title: 'DN Amount',
                data: 'dnamount',
                className: 'align-middle text-nowrap text-end',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatMoney(data, 0,".", ",");
                    } else {
                        return '0';
                    }
                }
            },
            {
                targets: 7,
                title: 'Fee Description',
                data: 'feedesc',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 8,
                title: 'Fee Amount',
                data: 'feeamount',
                className: 'align-middle text-nowrap text-end',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatMoney(data, 0,".", ",");
                    } else {
                        return '0';
                    }
                }
            },
            {
                targets: 9,
                title: 'DPP',
                data: 'dpp',
                className: 'align-middle text-nowrap text-end',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatMoney(data, 0,".", ",");
                    } else {
                        return '0';
                    }
                }
            },
            {
                targets: 10,
                title: 'PPN',
                data: 'ppnamt',
                className: 'align-middle text-nowrap text-end',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatMoney(data, 0,".", ",");
                    } else {
                        return '0';
                    }
                }
            },
            {
                targets: 11,
                title: 'PPH',
                data: 'pphamt',
                className: 'align-middle text-nowrap text-end',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatMoney(data, 0,".", ",");
                    } else {
                        return '0';
                    }
                }
            },
            {
                targets: 12,
                title: 'DN Claim',
                data: 'totalclaim',
                className: 'align-middle text-nowrap text-end',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatMoney(data, 0,".", ",");
                    } else {
                        return '0';
                    }
                }
            },
            {
                targets: 13,
                title: 'DN Paid',
                data: 'totalpaid',
                className: 'align-middle text-nowrap text-end',
                render: function (data, type, full, meta) {
                    if (data !== "") {
                        return formatMoney(data, 0,".", ",");
                    } else {
                        return '0';
                    }
                }
            },
            {
                targets: 14,
                title: 'File 1',
                data: 'row1',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 15,
                title: 'File 2',
                data: 'row2',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 16,
                title: 'File 3',
                data: 'row3',
                className: 'align-middle text-nowrap',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        console.log(e.jqXHR);
    };
});

$('#dt_workflow_history_search').on('keyup', function() {
    dt_workflow_history.search(this.value, false, false).draw();
});

$('#dt_change_history_search').on('keyup', function() {
    dt_change_history.search(this.value, false, false).draw();
});

$('#dt_dn_list_search').on('keyup', function() {
    dt_dn_list.search(this.value, false, false).draw();
});

$('#btn_print').on('click', function () {
    if (promoId) {
        window.open('/promo/workflow/print-pdf?id=' + promoId + '&refId=' + refId +'&statusMechanismList=' + statusMechanismList + '&c=' + categoryDesc + '&sy=' + yearPromo, '_blank');
    }
});

$('#btn_view').on('click', async function () {
    let btn = document.getElementById('btn_view');
    let refId = $('#filter_promo_id').val();

    if (refId !== "") {
        btn.setAttribute("data-kt-indicator", "on");
        btn.disabled = !0;
        blockUIDataPromoRevamp.block();
        blockUIDataPromo.block();
        blockUIDataPromoDC.block();
        blockUIWorkflowHistory.block();
        blockUIChangeHistory.block();
        blockUIDNList.block();

        dt_workflow_history.clear().draw();
        dt_change_history.clear().draw();
        dt_dn_list.clear().draw();

        if (refId !== "") await getDataPromo(refId);

        if ($('#workflow_history').is(':checked') && refId !== "") {
            dt_workflow_history.ajax.url('/promo/workflow/workflow-history?refId=' + refId).load(function () {
                blockUIWorkflowHistory.release();
            });
        }

        if ($('#change_history').is(':checked') && refId !== "") {
            dt_change_history.ajax.url('/promo/workflow/change-history?refId=' + refId).load(function () {
                blockUIChangeHistory.release();
            });

            dt_dn_list.ajax.url('/promo/workflow/list-dn?refId=' + refId).load(function () {
                blockUIDNList.release();
            });
        }

        btn.setAttribute("data-kt-indicator", "off");
        btn.disabled = !1;

        blockUIDataPromoRevamp.release();
        blockUIDataPromo.release();
        blockUIDataPromoDC.release();
    } else {
        Swal.fire({
            title: swalTitle,
            text: "Please fill in Promo ID",
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "OK",
            customClass: {confirmButton: "btn btn-optima"},
            allowOutsideClick: false,
            allowEscapeKey: false,
        });
    }
});

$('#workflow_history').on('change', function () {
    if ($(this).is(':checked')) {
        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
        $('#row_workflow_history').removeClass('d-none');
    } else {
        $('#row_workflow_history').addClass('d-none');
    }
});

$('#change_history').on('change', function () {
    if ($(this).is(':checked')) {
        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
        $('#row_change_history').removeClass('d-none');
        $('#row_dn_list').removeClass('d-none');
    } else {
        $('#row_change_history').addClass('d-none');
        $('#row_dn_list').addClass('d-none');
    }
});

$('.btn_download_revamp').on('click', function() {
    let row = $(this).val();
    let attachment;
    if (row !== "all") {
        if (categoryDesc === "Distributor Cost") {
            attachment = $('#review_file_label_dc_new_' + row.slice(-1));
        } else {
            attachment = $('#review_file_label_rc_new_' + row.slice(-1));
        }
        let file_name = attachment.val();
        let url = file_host + "/assets/media/promo/" + promoId + "/" + row + "/" + file_name;
        if (attachment.val() !== "") {
            fetch(url)
                .then((resp) => {
                    if (resp.ok) {
                        resp.blob().then(blob => {
                            const url_blob = window.URL.createObjectURL(blob);
                            const a = document.createElement('a');
                            a.style.display = 'none';
                            a.href = url_blob;
                            a.download = file_name;
                            document.body.appendChild(a);
                            a.click();
                            window.URL.revokeObjectURL(url_blob);
                        })
                            .catch(e => {
                                console.log(e);
                                Swal.fire({
                                    text: "Download attachment failed",
                                    icon: "warning",
                                    buttonsStyling: !1,
                                    confirmButtonText: "OK",
                                    allowOutsideClick: false,
                                    allowEscapeKey: false,
                                    customClass: {confirmButton: "btn btn-optima"},
                                });
                            });
                    } else {
                        Swal.fire({
                            title: "Download Attachment",
                            text: "Attachment not found",
                            icon: "warning",
                            buttonsStyling: !1,
                            confirmButtonText: "OK",
                            allowOutsideClick: false,
                            allowEscapeKey: false,
                            customClass: {confirmButton: "btn btn-optima"},
                        });
                    }
                });
        } else {
            Swal.fire({
                title: "Download Attachment",
                text: "Attachment not found",
                icon: "warning",
                buttonsStyling: !1,
                confirmButtonText: "OK",
                customClass: {confirmButton: "btn btn-optima"},
                allowOutsideClick: false,
            });
        }
    }
});

$('.btn_view_revamp').on('click', function () {
    let row = $(this).val();
    let attachment;
    if (categoryDesc === "Distributor Cost") {
        attachment = $('#review_file_label_dc_new_' + row);
    } else {
        attachment = $('#review_file_label_rc_new_' + row);
    }
    let file_name = attachment.val();
    let title = $('#txt_info_method').text();
    let preview_path =  "/promo/workflow/preview-attachment?i="+promoId+"&r="+row+"&fileName="+encodeURIComponent(file_name)+"&t="+title;
    let w = window.innerWidth;
    let h = window.innerHeight;
    window.open(preview_path, "promoWindow", "location=1,status=1,scrollbars=1,width=" + w + ",height=" + h);
});

$('.btn_download').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let attachment = $('#row' + row);
        if (categoryDesc === "Distributor Cost") attachment = $('#row' + row + "DC");
        let url = file_host + "/assets/media/promo/" + promoId + "/row" + row + "/" + attachment.val();
        if (attachment.val() !== "") {
            fetch(url).then((resp) => {
                blockUIDataPromo.block();
                blockUIDataPromoDC.block();
                if (resp.ok) {
                    resp.blob().then(blob => {
                        const url_blob = window.URL.createObjectURL(blob);
                        const a = document.createElement('a');
                        a.style.display = 'none';
                        a.href = url_blob;
                        a.download = attachment.val();
                        document.body.appendChild(a);
                        a.click();
                        window.URL.revokeObjectURL(url_blob);
                        blockUIDataPromo.release();
                        blockUIDataPromoDC.release();
                    })
                    .catch(e => {
                        blockUIDataPromo.release();
                        blockUIDataPromoDC.release();
                        console.log(e);
                        Swal.fire({
                            text: "Download attachment failed",
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
                    blockUIDataPromo.release();
                    blockUIDataPromoDC.release();
                    Swal.fire({
                        title: "Download Attachment",
                        text: "Attachment not found",
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
        }
    }
});

$('#txt_workflow_history').on('dblclick', async function () {
    if (refId) {
        blockUIDataPromo.block();
        blockUIDataPromoDC.block();
        blockUIWorkflowHistory.block();
        blockUIChangeHistory.block();
        blockUIDNList.block();
        $('#approval_workflow_ref_id').text(refId);
        await getDataApprovalWorkflow(refId).then(function () {
            blockUIDataPromo.release();
            blockUIDataPromoDC.release();
            blockUIWorkflowHistory.release();
            blockUIChangeHistory.release();
            blockUIDNList.release();
            let drawerElement = document.querySelector("#drawer_timeline");
            let drawer = KTDrawer.getInstance(drawerElement);
            drawer.toggle();
        });
    } else {
        let drawerElement = document.querySelector("#drawer_timeline");
        let drawer = KTDrawer.getInstance(drawerElement);
        drawer.toggle();
    }
});

const getDataPromo = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/workflow/data",
            type: "GET",
            data: {refId:id},
            dataType: "JSON",
            success: async function (result) {
                if (!result.error) {
                    let values = result.data;
                    let data;
                    resetForm();

                    if (values.statusPromo) {
                        if (values.statusPromo.length > 0) {
                            let status = values.statusPromo[0];
                            let elFlowOngoingApproval = $('#flow_ongoing_approval');
                            let elFlowFullyApproved = $('#flow_fully_approved');
                            let elFlowClaimProcess = $('#flow_claim_process');
                            let elFlowClosed = $('#flow_closed');
                            let elFlowCanceled = $('#flow_canceled');

                            elFlowOngoingApproval.removeClass('active-flow');
                            elFlowFullyApproved.removeClass('active-flow');
                            elFlowClaimProcess.removeClass('active-flow');
                            elFlowClosed.removeClass('active-flow');
                            elFlowCanceled.removeClass('active-flow');

                            if (status.ongoingapproval !== 0) {
                                elFlowOngoingApproval.addClass('active-flow');
                                $('#txt_ongoing_approval').text('ONGOING APPROVAL ('+ status.ongoingapproval + ')');
                            }
                            if (status.fullyapproved !== 0) {
                                elFlowFullyApproved.addClass('active-flow');
                                $('#txt_fully_approved').text('FULLY APPROVED ('+ status.fullyapproved + ')');
                            }
                            if (status.claimprocess !== 0) {
                                elFlowClaimProcess.addClass('active-flow');
                                $('#txt_claim_process').text('CLAIM PROCESS ('+ status.claimprocess + ')');
                            }
                            if (status.closed !== 0) {
                                elFlowClosed.addClass('active-flow');
                                $('#txt_closed').text('CLOSED ('+ status.closed + ')');
                            }
                            if (status.cancelled !== 0) {
                                elFlowCanceled.addClass('active-flow');
                                $('#txt_canceled').text('CANCELED ('+ status.cancelled + ')');
                            }
                        }
                    }

                    if (values.promo) {
                        if (values.promo.length > 0) {
                            data = values.promo[0];

                            promoId = data.id;
                            refId = data.refId;
                            $('#txt_info_method').text(data.refId + ' - ' + data.laststatus);

                            yearPromo = new Date(data.startPromo).getFullYear();
                            categoryDesc = data.categoryDesc;
                            if (categoryDesc === "Distributor Cost") {
                                if (data.periode >= 2025) {
                                    await getDataPromoV2(id, categoryDesc);
                                } else {
                                    $('#card_form_dc').removeClass('d-none');
                                    $('#card_form_rc').addClass('d-none');
                                    $('#card_form_rc_new').addClass('d-none');
                                    $('#card_form_dc_new').addClass('d-none');

                                    $('#periodDC').val(data.periode);
                                    $('#entityDC').val(data.principalName);
                                    $('#distributorDC').val(data.distributorName);
                                    $('#subCategoryDC').val(data.subCategoryDesc);
                                    $('#subActivityDC').val(data.subActivityLongDesc);

                                    $('#allocationRefIdDC').val(data.allocationRefId);
                                    $('#startPromoDC').val(((data.startPromo) ? formatDate(data.startPromo) : ""));
                                    $('#endPromoDC').val(((data.endPromo) ? formatDate(data.endPromo) : ""));
                                    $('#investmentDC').val(formatMoney((data.investment ?? 0), 0));
                                    $('#initiatorNotesDC').val(data.initiator_notes);

                                    $('#allocationDescDC').val(data.allocationDesc);
                                    $('#budgetDeployedDC').val(formatMoney(data.budgetAmount,  0));
                                    $('#budgetRemainingDC').val(formatMoney(data.remainingBudget, 0));
                                    $('#totalClaimDC').val(formatMoney((data.totalClaim ?? 0), 0));
                                    $('#totalPaidDC').val(formatMoney((data.totalPaid ?? 0), 0));
                                }
                            } else {
                                if (data.periode >= 2025) {
                                    await getDataPromoV2(id, categoryDesc);
                                } else {
                                    $('#card_form_rc').removeClass('d-none');
                                    $('#card_form_dc').addClass('d-none');
                                    $('#card_form_rc_new').addClass('d-none');
                                    $('#card_form_dc_new').addClass('d-none');

                                    $('#periode').val(data.periode);
                                    $('#promoPlanRefId').val(data.promoPlanRefId);
                                    $('#allocationRefId').val(data.allocationRefId);
                                    $('#allocationDesc').val(data.allocationDesc);
                                    $('#activityLongDesc').val(data.activityLongDesc);
                                    $('#subActivityLongDesc').val(data.subActivityLongDesc);
                                    $('#activityDesc').val(data.activityDesc);
                                    $('#startPromo').val(((data.startPromo) ? formatDate(data.startPromo) : ""));
                                    $('#endPromo').val(((data.endPromo) ? formatDate(data.endPromo) : ""));
                                    $('#tsCoding').val(data.tsCoding);
                                    $('#subCategoryDesc').val(data.subCategoryDesc);
                                    $('#principalName').val(data.principalName);
                                    $('#distributorName').val(data.distributorName);
                                    $('#budgetAmount').val(formatMoney((data.budgetAmount ?? 0), 0));
                                    $('#remainingBudget').val(formatMoney((data.remainingBudget ?? 0), 0));
                                    $('#totalClaim').val(formatMoney((data.totalClaim ?? 0), 0));
                                    $('#totalPaid').val(formatMoney((data.totalPaid ?? 0), 0));
                                    $('#normalSales').val(formatMoney((data.normalSales ?? 0), 0));
                                    $('#incrSales').val(formatMoney((data.incrSales ?? 0), 0));
                                    $('#investment').val(formatMoney((data.investment ?? 0), 0));
                                    let totSales = parseFloat(((data.normalSales ?? 0)) + parseFloat((data.incrSales ?? 0)));
                                    $('#totSales').val(formatMoney(totSales.toString(), 0));
                                    $('#totInvestment').val(formatMoney((data.investment ?? 0), 0));
                                    $('#roi').val(formatMoney((data.roi ?? 0), 2));
                                    $('#costRatio').val(formatMoney((data.costRatio ?? 0), 2));
                                }
                            }
                        }
                    }
                    if (categoryDesc === "Distributor Cost") {
                        if (values.channel) {
                            let strChannel = "";
                            let channel = values.channel;
                            if (channel.length > 0) {
                                let arrChannels = [];
                                for (let i=0; i<channel.length; i++) {
                                    if (channel[i].flag) {
                                        arrChannels.push(channel[i].longDesc);
                                    }
                                }
                                if(arrChannels.length > 0){
                                    strChannel += arrChannels.reduce((text, value, i, array) => text + (i < array.length - 1 ? ', ' : ', ') + value);
                                }
                            }
                            $('#channelDC').val(strChannel);
                        }
                    } else {
                        if (data.periode < '2025') {
                            if (values.channel) {
                                let channel = values.channel;
                                if (channel.length > 0) {
                                    for (let i=0; i<channel.length; i++) {
                                        if (channel[i].flag) {
                                            $('#channel').val(channel[i].longDesc);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (categoryDesc === "Distributor Cost") {
                        // Group Brand
                        if (values.groupBrand) {
                            for (let i = 0; i < values.groupBrand.length; i++) {
                                if (values.groupBrand[i].flag) {
                                    $('#brandDC').val(values.groupBrand[i].longDesc);
                                }
                            }
                        }
                        if (values.groupBrands) {
                            for (let i = 0; i < values.groupBrands.length; i++) {
                                if (values.groupBrands[i].flag) {
                                    $('#brandDC').val(values.groupBrands[i].longDesc);
                                }
                            }
                        }
                    } else {
                        if (data.periode < '2025') {
                            if (values.subChannel) {
                                let subChannel = values.subChannel;
                                if (subChannel.length > 0) {
                                    for (let i=0; i<subChannel.length; i++) {
                                        if (subChannel[i].flag) {
                                            $('#subChannel').val(subChannel[i].longDesc);
                                        }
                                    }
                                }
                            }

                            if (values.account) {
                                let account = values.account;
                                if (account.length > 0) {
                                    for (let i=0; i<account.length; i++) {
                                        if (account[i].flag) {
                                            $('#account').val(account[i].longDesc);
                                        }
                                    }
                                }
                            }

                            if (values.subAccount) {
                                let subAccount = values.subAccount;
                                if (subAccount.length > 0) {
                                    for (let i=0; i<subAccount.length; i++) {
                                        if (subAccount[i].flag) {
                                            $('#subAccount').val(subAccount[i].longDesc);
                                        }
                                    }
                                }
                            }

                            // Region
                            let longDescRegion = '';
                            if (values.region) {
                                let region = values.region;
                                if (region.length > 0) {
                                    for (let i=0; i<region.length; i++) {
                                        if (region[i].flag) {
                                            longDescRegion += '<span class="fw-bold text-sm-left">' + region[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                                        }
                                    }
                                    $('#card_list_region').html(longDescRegion);
                                }
                            }

                            // Brand
                            let longDescBrand = '';
                            if (values.brand) {
                                let brand = values.brand;
                                if (brand.length > 0) {
                                    for (let i=0; i<brand.length; i++) {
                                        if (brand[i].flag) {
                                            longDescBrand += '<span class="fw-bold text-sm-left">' + brand[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                                        }
                                    }
                                    $('#card_list_brand').html(longDescBrand);
                                }
                            }

                            // SKU
                            let longDescSKU = '';
                            if (values.sku) {
                                let sku = values.sku;
                                if (sku.length > 0) {
                                    for (let i=0; i<sku.length; i++) {
                                        if (sku[i].flag) {
                                            longDescSKU += '<span class="fw-bold text-sm-left">' + sku[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                                        }
                                    }
                                    $('#card_list_sku').html(longDescSKU);
                                }
                            }
                        }
                    }

                    if (categoryDesc === "Distributor Cost") {
                        if (values.mechanisms) {
                            for (let i = 0; i < values.mechanisms.length; i++) {
                                $('#mechanism1DC').val(values.mechanisms[i].mechanism);
                            }
                        }
                        if (values.mechanism) {
                            for (let i = 0; i < values.mechanism.length; i++) {
                                $('#mechanism1DC').val(values.mechanism[i].mechanism);
                            }
                        }
                    } else {
                        if (data.periode < '2025') {
                            if (values.mechanism) {
                                dt_mechanism.clear().draw();
                                dt_mechanism.rows.add(values.mechanism).draw();
                            }
                        }
                    }

                    if (categoryDesc === "Distributor Cost") {
                        if (values.fileAttach) {
                            let fileAttach = values.fileAttach;
                            if (fileAttach.length > 0) {
                                for (let i = 0; i < fileAttach.length; i++) {
                                    let cnt_row = (fileAttach[i].docLink ?? '').replace('row', '');
                                    $('#' + fileAttach[i].docLink + "DC").val(fileAttach[i].fileName);
                                    $('#btn_download' + cnt_row + "DC").removeClass('invisible');
                                }
                            }
                        }
                        if (values.attachments) {
                            values.attachments.forEach((item, index, arr) => {
                                if (item.docLink === 'row' + parseInt(item.docLink.replace('row', ''))) {
                                    let cnt_row = (item.docLink ?? '').replace('row', '');
                                    $('#' + fileAttach[i].docLink + "DC").val(fileAttach[i].fileName);
                                    $('#btn_download' + cnt_row + "DC").removeClass('invisible');
                                }
                            });
                        }
                    } else {
                        if (data.periode < '2025') {
                            if (values.fileAttach) {
                                let fileAttach = values.fileAttach;
                                if (fileAttach.length > 0) {
                                    for (let i = 0; i < fileAttach.length; i++) {
                                        let cnt_row = (fileAttach[i].docLink ?? '').replace('row', '');
                                        $('#' + fileAttach[i].docLink).val(fileAttach[i].fileName);
                                        $('#btn_download' + cnt_row).removeClass('invisible');
                                    }
                                }
                            }
                        }
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
            complete:function(){
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getDataPromoV2 = (id, categoryDesc) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/workflow/data-revamp/refId",
            type: "GET",
            data: {refid:id},
            dataType: "JSON",
            success: async function (result) {
                if (!result['error']) {
                    let data = result['data'];
                    let promo = data['promo'];
                    let dn = data['dn'];

                    if (categoryDesc === "Distributor Cost") {
                        $('#card_form_dc_new').removeClass('d-none');
                        $('#card_form_rc_new').addClass('d-none');
                        $('#card_form_rc').addClass('d-none');
                        $('#card_form_dc').addClass('d-none');

                        $('#periodDCNew').val(promo['period']);
                        $('#entityLongDescDCNew').val(promo['entityLongDesc']);
                        $('#groupBrandLongDescDCNew').val(promo['groupBrandLongDesc']);
                        $('#distributorLongDescDCNew').val(promo['distributorLongDesc']);
                        $('#subActivityTypeDescDCNew').val(promo['subActivityType']);
                        $('#subActivityLongDescDCNew').val(promo['subActivityLongDesc']);
                        $('#channelDescDCNew').val(promo['channelDesc']);
                        $('#startPromoDCNew').val(formatDateOptima(promo['startPromo']));
                        $('#endPromoDCNew').val(formatDateOptima(promo['endPromo']));
                        $('#initiatorNotesDCNew').val(promo['initiator_notes']);

                        $('#budgetSourceNameDCNew').val(promo['budgetName']);
                        $('#remainingBudgetDCNew').val((promo['remainingBudget'] ? formatMoney(promo['remainingBudget'],0) : 0));
                        $('#totalCostDCNew').val((promo['cost'] ? formatMoney(promo['cost'],0) : 0));
                        $('#totalClaimDCNew').val((dn['totalClaim'] ? formatMoney(dn['totalClaim'],0) : 0));
                        $('#totalPaidDCNew').val((dn['totalPaid'] ? formatMoney(dn['totalPaid'],0) : 0));

                        if (data['mechanism'].length > 0) {
                            $('#mechanismDCNew').val(data['mechanism'][0]['mechanism']);
                        }

                        if (data['attachment']) {
                            data['attachment'].forEach((item) => {
                                if (item['docLink'] === 'row' + parseInt(item['docLink'].replace('row', ''))) {
                                    $('#review_file_label_dc_new_' + parseInt(item['docLink'].replace('row', ''))).val(item.fileName);
                                    $('#btn_download_dc_new' + parseInt(item['docLink'].replace('row', ''))).val(item['docLink']);
                                    $('#btn_download_dc_new' + parseInt(item['docLink'].replace('row', ''))).attr('disabled', false);
                                    $('#btn_view_dc_new' + parseInt(item['docLink'].replace('row', ''))).attr('disabled', false);
                                }
                            });
                        }
                        statusMechanismList = promo['mechanismInputMethod'];
                    } else {
                        $('#card_form_rc_new').removeClass('d-none');
                        $('#card_form_dc_new').addClass('d-none');
                        $('#card_form_rc').addClass('d-none');
                        $('#card_form_dc').addClass('d-none');

                        statusMechanismList = promo['mechanismInputMethod'];

                        $('#entityLongDescRCNew').val(promo['entityLongDesc']);
                        $('#groupBrandLongDescRCNew').val(promo['groupBrandLongDesc']);
                        $('#subCategoryLongDescRCNew').val(promo['subCategoryLongDesc']);
                        $('#activityLongDescRCNew').val(promo['activityLongDesc']);
                        $('#subActivityLongDescRCNew').val(promo['subActivityLongDesc']);
                        $('#activityDescRCNew').val(promo['activityDesc']);
                        $('#startPromoRCNew').val(formatDateOptima(promo['startPromo']));
                        $('#endPromoRCNew').val(formatDateOptima(promo['endPromo']));
                        $('#periodRCNew').val(promo['period']);
                        $('#distributorLongDescRCNew').val(promo['distributorLongDesc']);
                        $('#channelDescRCNew').val(promo['channelDesc']);
                        $('#subChannelDescRCNew').val(promo['subChannelDesc']);
                        $('#accountDescRCNew').val(promo['accountDesc']);
                        $('#subAccountDescRCNew').val(promo['subAccountDesc']);


                        $('#budgetSourceNameRCNew').val(promo['budgetName']);
                        $('#remainingBudgetRCNew').val((promo['remainingBudget'] ? formatMoney(promo['remainingBudget'],0) : 0));
                        $('#totalCostRCNew').val((promo['cost'] ? formatMoney(promo['cost'],0) : 0));
                        $('#totalClaimRCNew').val((dn['totalClaim'] ? formatMoney(dn['totalClaim'],0) : 0));
                        $('#totalPaidRCNew').val((dn['totalPaid'] ? formatMoney(dn['totalPaid'],0) : 0));

                        // set data region
                        let arrSetRegion = [];
                        for (let i=0; i<data['region'].length; i++) {
                            arrSetRegion.push(data['region'][i]['regionDesc']);
                        }
                        let regionText = (arrSetRegion.join(', ') ?? '-');
                        $('#txtInfoRegion').text(regionText);

                        dt_sku.clear().draw();
                        dt_sku.rows.add(data['sku']).draw();

                        if (data['mechanism'].length > 0) {
                            for (let i=0; i<data['mechanism'].length; i++) {
                                data['mechanism'][i]['no'] = i+1;
                            }
                        }
                        dt_mechanism_revamp.clear().draw();
                        dt_mechanism_revamp.rows.add(data['mechanism']).draw();

                        if (promo['mechanismInputMethod']) {
                            dt_mechanism_revamp.column(3).visible(true);
                        } else {
                            dt_mechanism_revamp.column(3).visible(false);
                        }

                        if (data['attachment']) {
                            data['attachment'].forEach((item) => {
                                if (item['docLink'] === 'row' + parseInt(item['docLink'].replace('row', ''))) {
                                    $('#review_file_label_rc_new_' + parseInt(item['docLink'].replace('row', ''))).val(item.fileName);
                                    $('#btn_download_rc_new' + parseInt(item['docLink'].replace('row', ''))).val(item['docLink']);
                                    $('#btn_download_rc_new' + parseInt(item['docLink'].replace('row', ''))).attr('disabled', false);
                                    $('#btn_view_revamp' + parseInt(item['docLink'].replace('row', ''))).attr('disabled', false);
                                }
                            });
                        }
                    }
                }
            },
            complete:function(){
                return resolve();
            },
            error: function (jqXHR) {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getDataApprovalWorkflow = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/workflow/approval-workflow",
            type: "GET",
            data: {refId:id},
            dataType: "JSON",
            success: async function (result) {
                if (!result.error) {
                    let result_timeline = result.data;
                    let html_timeline = '';

                    if (result_timeline) {
                        if (result_timeline.length > 0) {
                            for (let i=0; i<result_timeline.length; i++) {
                                html_timeline += '' +
                                    '<div class="timeline-item">\n' +
                                    '   <div class="timeline-content d-flex flex-row">\n' +
                                    '       <div class="d-flex flex-column left-timeline">\n' +
                                    '           <span class="fw-bold text-gray-800 pe-3 text-end" style="'+ (result_timeline[i]['stylingSection1'] ?? "") +'">' + result_timeline[i]['section1'] + '</span>\n' +
                                    '           <span class="fw-bold text-gray-600 pe-3 text-end" style="'+ (result_timeline[i]['stylingSection2'] ?? "") +'">' + result_timeline[i]['section2'] + '</span>\n' +
                                    '           <span class="fw-bold text-gray-600 pe-3 text-end" style="'+ (result_timeline[i]['stylingSection3'] ?? "") +'">' + result_timeline[i]['section3'] + '</span>\n' +
                                    '           <span class="fw-bold text-gray-600 pe-3 text-end" style="'+ (result_timeline[i]['stylingSection4'] ?? "") +'">' + result_timeline[i]['section4'] + '</span>\n' +
                                    '       </div>\n' +
                                    '       <div class="timeline-badge center-timeline">\n' +
                                    '           <i class="fa fa-genderless text-optima fs-1" style="'+ (result_timeline[i]['stylingSectionCircle'] ?? "") +'"></i>\n' +
                                    '       </div>\n' +
                                    '       <div class="d-flex flex-column right-timeline">\n' +
                                    '           <span class="fw-bold text-gray-600 ps-3" style="'+ (result_timeline[i]['stylingSection5'] ?? "") +'">' + result_timeline[i]['section5'] + '</span>\n' +
                                    '           <span class="fw-bold text-gray-600 ps-3" style="'+ (result_timeline[i]['stylingSection6'] ?? "") +'">' + result_timeline[i]['section6'] + '</span>\n' +
                                    '       </div>\n' +
                                    '   </div>\n' +
                                    '</div>';
                            }
                        }
                    }
                    $('#list_timeline').html(html_timeline);
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
            complete:function(){
                return resolve();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const resetForm = () => {
    $('#txt_ongoing_approval').text('ONGOING APPROVAL');
    $('#txt_fully_approved').text('FULLY APPROVED');
    $('#txt_claim_process').text('CLAIM PROCESS');
    $('#txt_closed').text('CLOSED');
    $('#txt_canceled').text('CANCELED');

    $('.review_file_label_revamp').val('');
    $('.btn_download_revamp').val('');
    $('.btn_download_revamp').attr('disabled', true);
    $('.btn_view_revamp').attr('disabled', true);
}
