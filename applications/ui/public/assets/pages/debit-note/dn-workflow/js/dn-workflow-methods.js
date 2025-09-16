'use strict';

var dt_workflow_history, dt_change_history;
var swalTitle = "DN Workflow";
var dnId;

const targetDataDN = document.querySelector(".card_data_dn");
const blockUIDataDN = new KTBlockUI(targetDataDN, {
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

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
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
                data: 'strdtime',
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
                title: 'Last Status',
                data: 'status',
                className: 'align-middle text-nowrap',
            },
            {
                targets: 4,
                title: 'DPP',
                data: 'dpp',
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
                title: 'Action',
                data: 'action',
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
                data: 'dtime',
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

    $.fn.dataTable.ext.errMode = function (e, settings, helpPage, message) {
        console.log(e.jqXHR);
    };
});

$('#btn_view').on('click', async function () {
    let btn = document.getElementById('btn_view');
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;

    let refId = $('#filter_dn_id').val();

    blockUIDataDN.block();
    blockUIWorkflowHistory.block();
    blockUIChangeHistory.block();

    if (refId !== "") await getDataDN(refId);

    if ($('#workflow_history').is(':checked') && refId !== "") {
        dt_workflow_history.clear().draw();
        dt_workflow_history.ajax.url('/dn/workflow/workflow-history?refId=' + refId).load();
    }

    if ($('#change_history').is(':checked') && refId !== "") {
        dt_change_history.clear().draw();
        dt_change_history.ajax.url('/dn/workflow/change-history?refId=' + refId).load();
    }

    btn.setAttribute("data-kt-indicator", "off");
    btn.disabled = !1;

    blockUIDataDN.release();
    blockUIWorkflowHistory.release();
    blockUIChangeHistory.release();
});

$('#dt_workflow_history_search').on('keyup', function() {
    dt_workflow_history.search(this.value).draw();
});

$('#dt_change_history_search').on('keyup', function() {
    dt_change_history.search(this.value).draw();
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
    } else {
        $('#row_change_history').addClass('d-none');
    }
});

$('.btn_download').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let attachment = $('#row' + row);
        let url = file_host + "/assets/media/debitnote/" + dnId + "/row" + row + "/" + attachment.val();
        blockUIDataDN.block();
        if (attachment.val() !== "") {
            fetch(url)
                .then((resp) => {
                    if (resp.ok) {
                        resp.blob().then(blob => {
                            const url_blob = window.URL.createObjectURL(blob);
                            const a = document.createElement('a');
                            a.style.display = 'none';
                            a.href = url_blob;
                            a.download = $('#row' + row).val();
                            document.body.appendChild(a);
                            a.click();
                            window.URL.revokeObjectURL(url_blob);
                            blockUIDataDN.release();
                        })
                        .catch(e => {
                            blockUIDataDN.release();
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
                        blockUIDataDN.release();
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

$('#btn_export_excel').on('click', function () {
    if (dnId) {
        window.open('/dn/workflow/print-pdf?&id=' + dnId, '_blank');
    }
});

const getDataDN = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/dn/workflow/data",
            type: "GET",
            data: {refId:id},
            dataType: "JSON",
            success: async function (result) {
                if (!result.error) {
                    let values = result.data;

                    resetForm();

                    if (values.statusresult) {
                        if (values.statusresult.length > 0) {
                            let status = values.statusresult[0];
                            let elFlowDistributor = $('#flow_distributor');
                            let elFlowWaitingValidation = $('#flow_waiting_validation');
                            let elFlowPaymentProcess = $('#flow_payment_process');
                            let elFlowPaid = $('#flow_paid');
                            let elFlowCanceled = $('#flow_canceled');

                            elFlowDistributor.removeClass('active-flow');
                            elFlowWaitingValidation.removeClass('active-flow');
                            elFlowPaymentProcess.removeClass('active-flow');
                            elFlowPaid.removeClass('active-flow');
                            elFlowCanceled.removeClass('active-flow');

                            if (status.distributor) elFlowDistributor.addClass('active-flow');
                            if (status.waiting_Validation) elFlowWaitingValidation.addClass('active-flow');
                            if (status.payment_Process) elFlowPaymentProcess.addClass('active-flow');
                            if (status.paid) elFlowPaid.addClass('active-flow');
                            if (status.cancelled) elFlowCanceled.addClass('active-flow');
                        }
                    }

                    if (values.debetnoteresult) {
                        if (values.debetnoteresult.length > 0) {
                            let data = values.debetnoteresult[0];

                            dnId = data.id;
                            $('#txt_info_method').text(data.refId + ' - ' + data.lastStatus);
                            $('#year').val(data.periode);
                            $('#subAccount').val(data.accountDesc);
                            if (values.sellingpointresult.length > 0) $('#sellingPoint').val(values.sellingpointresult[0].longDesc);
                            $('#promoRefId').val(data.promoRefId);
                            $('#entity').val(data.entityLongDesc);
                            $('#entityAddress').val(data.entityAddress);
                            $('#entityUp').val(data.entityUp);
                            $('#activityDesc').val(data.activityDesc);
                            $('#feeDesc').val(data.feeDesc);
                            $('#fpNumber').val(data.fpNumber);
                            $('#fpDate').val(  ((data.fpNumber) ? formatDate(data.fpDate) : '') );
                            $('#taxLevel').val(data.taxLevel);
                            $('#whtType').val(data.whtType);
                            $('#vatExpired').val( ((data.vatExpired) ? 'Yes' : 'No') );
                            $('#isDNPromo').val( ((data.isDNPromo) ? 'Yes' : 'No') );
                            $('#deductionDate').val( ((data.deductionDate) ? formatDate(data.deductionDate) : '') );
                            $('#memDocNo').val(data.memDocNo);
                            $('#intDocNo').val(data.intDocNo);
                            $('#period').val((data.startPromo) ? formatDate(data.startPromo) + " to " + formatDate(data.endPromo) : '');
                            $('#dnAmount').val( formatMoney(data.dnAmount, 0) );
                            $('#feePct').val( formatMoney(data.feePct, 2) );
                            $('#feeAmount').val( formatMoney(data.feeAmount, 0) );
                            $('#dpp').val( formatMoney(data.dpp, 0) );

                            if(data.statusPPN == 'null') {
                                $('#statusPPN').val('');
                            } else if (data.statusPPN == null) {
                                $('#statusPPN').val('');
                            } else {
                                $('#statusPPN').val(data.statusPPN);
                            }

                            $('#ppnPct').val( formatMoney(data.ppnPct, 2) );
                            $('#ppnAmount').val( formatMoney(data.ppnAmt, 0) );

                            if(data.statusPPH == 'null') {
                                $('#statusPPH').val('');
                            } else if (data.statusPPH == null) {
                                $('#statusPPH').val('');
                            } else {
                                $('#statusPPH').val(data.statusPPH);
                            }

                            $('#pphPct').val( formatMoney(data.pphPct, 2) );
                            $('#pphAmount').val( formatMoney(data.pphAmt, 0) );
                            $('#total').val(formatMoney(data.totalClaim, 0));
                        }
                    }

                    if (values.fileattactresult) {
                        if (values.fileattactresult.length > 0) {
                            let fileAttachment = values.fileattactresult;
                            for (let i=0; i<fileAttachment.length; i++) {
                                if (fileAttachment[i].doclink !== "") {
                                    $('#'+fileAttachment[i].doclink).val(fileAttachment[i].fileName);
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

const resetForm = () => {
    $('#form_dn')[0].reset();
}
