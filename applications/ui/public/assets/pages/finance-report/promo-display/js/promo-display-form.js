'use strict';

var swalTitle = "DN Display";
var dt_mechanism;
var heightContainer = 700;
var id, arrSellingPoint = [], arrTaxLevel = [];

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    let url_str = new URL(window.location.href);
    id = url_str.searchParams.get("id");

    blockUI.block();
    Promise.all([ getData(id) ]).then(async () => {
        blockUI.release();
    });

    dt_mechanism = $('#dt_mechanism').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        ordering: false,
        processing: true,
        paging: false,
        searching: true,
        scrollX: true,
        autoWidth: false,
        lengthMenu: [[10, 20, 50, 100], [10, 20, 50, 100]],
        deferRender: true,
        columnDefs: [
            {
                title: 'No',
                targets: 0,
                data: 'mechanismId',
                width: 50,
                orderable: false,
                className: 'text-nowrap text-center align-middle',
                render: function (data, type, full, meta) {
                    return meta.row + 1;
                }
            },
            {
                targets: 1,
                title: 'Mechanism',
                data: 'mechanism',
                className: 'text-nowrap align-middle',
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

$('.btn_download').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let attachment = $('#attachment' + row);
        let url = file_host + "/assets/media/promo/" + id + "/row" + row + "/" + attachment.val();
        if (attachment.val() !== "") {
            fetch(url)
                .then((resp) => {
                    if (resp.ok) {
                        resp.blob().then(blob => {
                            const url_blob = window.URL.createObjectURL(blob);
                            const a = document.createElement('a');
                            a.style.display = 'none';
                            a.href = url_blob;
                            a.download = $('#attachment' + row).val();
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

$('.btn_view').on('click', function() {
    let docLink = $(this).val();
    let fileName = $('#attachment' + docLink).val();

    let path = '/fin-rpt/promo-display/view-file?promoId=' + id + "&docLink=row" + docLink + "&fileName=" + encodeURIComponent(fileName);
    window.open(path,"_blank");
});

$('#btn_export_pdf').on('click', function() {
    window.open( "/fin-rpt/promo-display/export-pdf?id=" + id , "_blank");
});

const getData = (id) => {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/fin-rpt/promo-display/data/id",
            type: "GET",
            data: {id: id},
            dataType: 'json',
            async: true,
            success: function (result) {
                if (!result.error) {
                    let value = result.data;
                    $('#txt_info_method').text('Promo ID ' + value.promoHeader.refId + ' | ' + value.promoHeader.lastStatus);

                    let year = new Date(value.promoHeader.startPromo);
                    $('#year').val(year.getFullYear());
                    $('#promoPlanRefId').val(value.promoHeader.promoPlanRefId);
                    $('#allocationRefId').val(value.promoHeader.allocationRefId);
                    $('#allocationDesc').val(value.promoHeader.allocationDesc);
                    $('#activityLongDesc').val(value.promoHeader.activityLongDesc);
                    $('#subActivityLongDesc').val(value.promoHeader.subActivityLongDesc);
                    $('#startPromo').val(formatDateOptima(value.promoHeader.startPromo));
                    $('#endPromo').val(formatDateOptima(value.promoHeader.endPromo));
                    $('#activityDesc').val(value.promoHeader.activityDesc);

                    $('#tsCoding').val(value.promoHeader.tsCoding);
                    $('#subCategoryDesc').val(value.promoHeader.subCategoryDesc);
                    $('#principalName').val(value.promoHeader.principalName);
                    $('#distributorname').val(value.promoHeader.distributorName);
                    $('#budgetAmount').val(formatMoney(value.promoHeader.budgetAmount, 0));
                    $('#remainingBudget').val(formatMoney(value.promoHeader.remainingBudget, 0));
                    $('#initiator').val(value.promoHeader.initiator);
                    $('#initiator_notes').val(value.promoHeader.initiator_notes);
                    $('#investmentTypeDesc').val(value.promoHeader.investmentTypeDesc);

                    $('#normalSales').val(formatMoney(value.promoHeader.normalSales,0));
                    $('#incrSales').val(formatMoney(value.promoHeader.incrSales,0));
                    $('#investment').val(formatMoney(value.promoHeader.investment,0));
                    $('#investmentBfrClose').val(formatMoney(value.promoHeader.investmentBfrClose,0));
                    $('#investmentClosedBalance').val(formatMoney(value.promoHeader.investmentClosedBalance,0));

                    $('#totSales').val(formatMoney((value.promoHeader.normalSales) + (value.promoHeader.incrSales),0));
                    $('#totInvestment').val(formatMoney(value.promoHeader.investment,0));
                    $('#roi').val(formatMoney(value.promoHeader.roi,0));
                    $('#costRatio').val(formatMoney(value.promoHeader.costRatio,0));

                    if (value.channels) {
                        if (value.channels.length > 0) {
                            value.channels.forEach(function(el) {
                                if (el.flag) {
                                    $('#channelDesc').val(el.longDesc);
                                }
                            });
                        }
                    }

                    if (value.subChannels) {
                        if (value.subChannels.length > 0) {
                            value.subChannels.forEach(function(el) {
                                if (el.flag) {
                                    $('#subchannelDesc').val(el.longDesc);
                                }
                            });
                        }
                    }

                    if (value.accounts) {
                        if (value.accounts.length > 0) {
                            value.accounts.forEach(function(el) {
                                if (el.flag) {
                                    $('#accountDesc').val(el.longDesc);
                                }
                            });
                        }
                    }

                    if (value.subAccounts) {
                        if (value.subAccounts.length > 0) {
                            value.subAccounts.forEach(function(el) {
                                if (el.flag) {
                                    $('#subaccountDesc').val(el.longDesc);
                                }
                            });
                        }
                    }

                    let longDescRegion='';
                    if (value.regions) {
                        if (value.regions.length > 0) {
                            value.regions.forEach(function(el) {
                                if (el.flag) {
                                    longDescRegion += '<span className="fw-bold text-sm-left">' + el.longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                                }
                            });
                            $('#card_list_region').html(longDescRegion);
                        }
                    }

                    let longDescBrand='';
                    if (value.brands) {
                        if (value.brands.length > 0) {
                            value.brands.forEach(function(el) {
                                if (el.flag) {
                                    longDescBrand += '<span className="fw-bold text-sm-left">' + el.longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                                }
                            });
                            $('#card_list_brand').html(longDescBrand);
                        }
                    }

                    let longDescSku='';
                    if (value.skus) {
                        if (value.skus.length > 0) {
                            value.skus.forEach(function(el) {
                                if (el.flag) {
                                    longDescSku += '<span className="fw-bold text-sm-left">' + el.longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                                }
                            });
                            $('#card_list_sku').html(longDescSku);
                        }
                    }

                    dt_mechanism.rows.add(value.mechanisms).draw();

                    if (value.attachments) {
                        value.attachments.forEach((item, index, arr) => {
                            if (item.docLink === 'row' + parseInt(item.docLink.replace('row', ''))) $('#attachment' + parseInt(item.docLink.replace('row', ''))).val(item.fileName);
                        });
                    }
                }
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
