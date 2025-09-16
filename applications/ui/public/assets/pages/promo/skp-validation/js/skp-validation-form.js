'use strict';

let id, promoRefId;
let swalTitle = "SKP Validation";
let method, dt_mechanism, validator;
let skpDraftAvail=0, skpDraftAvailBfrAct60=0, entityDraftMatch=0, brandDraftMatch=0, periodDraftMatch=0, activityDescDraftMatch=0,
    mechanismDraftMatch=0, investmentDraftMatch=0, distributorDraftMatch=0, channelDraftMatch=0, storeNameDraftMatch=0;
let entityMatch=0, brandMatch=0, periodMatch=0, activityDescMatch=0, mechanismMatch=0, investmentMatch=0, skpSign7Match=0, distributorMatch=0, channelMatch=0, storeNameMatch=0;

let targetDetail = document.querySelector(".card_detail");
let blockUIDetail = new KTBlockUI(targetDetail, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetAttribute = document.querySelector(".card_attribute");
let blockUIAttribute = new KTBlockUI(targetAttribute, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

let targetAttachment = document.querySelector(".card_attachment");
let blockUIAttachment = new KTBlockUI(targetAttachment, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

$(document.body).delegate('[type="checkbox"][readonly="readonly"]', 'click', function(e) {
    e.preventDefault();
});

$(document).ready(function () {
    $('form').submit(false);

    let url_str = new URL(window.location.href);
    method = url_str.searchParams.get("method");
    id = url_str.searchParams.get("id");

    validator = FormValidation.formValidation(document.getElementById('form_skp_validation'), {
        fields: {
            approvalStatusCode: {
                validators: {
                    notEmpty: {
                        message: "This field is required"
                    },
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"}),
        }
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
        scrollY: "30vh",
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

    blockUI.block();
    blockUIDetail.block();
    blockUIAttribute.block();
    blockUIAttachment.block();

    getData(id).then(function () {
        if(method ==='validate') {
            if (url_str.searchParams.get("skpDraftAvail") === "on") skpDraftAvail = 1;
            if (url_str.searchParams.get("skpDraftAvailBfrAct60") === "on") skpDraftAvailBfrAct60 = 1;
            if (url_str.searchParams.get("entityDraftMatch") === "on") entityDraftMatch = 1;
            if (url_str.searchParams.get("brandDraftMatch") === "on") brandDraftMatch = 1;
            if (url_str.searchParams.get("periodDraftMatch") === "on") periodDraftMatch = 1;
            if (url_str.searchParams.get("activityDescDraftMatch") === "on") activityDescDraftMatch = 1;
            if (url_str.searchParams.get("mechanismDraftMatch") === "on") mechanismDraftMatch = 1;
            if (url_str.searchParams.get("investmentDraftMatch") === "on") investmentDraftMatch = 1;
            if (url_str.searchParams.get("distributorDraftMatch") === "on") distributorDraftMatch = 1;
            if (url_str.searchParams.get("channelDraftMatch") === "on") channelDraftMatch = 1;
            if (url_str.searchParams.get("storeNameDraftMatch") === "on") storeNameDraftMatch = 1;

            if (url_str.searchParams.get("entityMatch") === "on") entityMatch = 1;
            if (url_str.searchParams.get("brandMatch") === "on") brandMatch = 1;
            if (url_str.searchParams.get("periodMatch") === "on") periodMatch = 1;
            if (url_str.searchParams.get("activityDescMatch") === "on") activityDescMatch = 1;
            if (url_str.searchParams.get("mechanismMatch") === "on") mechanismMatch = 1;
            if (url_str.searchParams.get("investmentMatch") === "on") investmentMatch = 1;
            if (url_str.searchParams.get("skpSign7Match") === "on") skpSign7Match = 1;
            if (url_str.searchParams.get("distributorMatch") === "on") distributorMatch = 1;
            if (url_str.searchParams.get("channelMatch") === "on") channelMatch = 1;
            if (url_str.searchParams.get("storeNameMatch") === "on") storeNameMatch = 1;


            $('#skpDraftAvail').prop('checked', skpDraftAvail);
            $('#skpDraftAvailBfrAct60').prop('checked', skpDraftAvailBfrAct60);
            $('#entityDraftMatch').prop('checked', entityDraftMatch);
            $('#brandDraftMatch').prop('checked', brandDraftMatch);
            $('#periodDraftMatch').prop('checked', periodDraftMatch);
            $('#activityDescDraftMatch').prop('checked', activityDescDraftMatch);
            $('#mechanismDraftMatch').prop('checked', mechanismDraftMatch);
            $('#investmentDraftMatch').prop('checked', investmentDraftMatch);
            $('#distributorDraftMatch').prop('checked', distributorDraftMatch);
            $('#channelDraftMatch').prop('checked', channelDraftMatch);
            $('#storeNameDraftMatch').prop('checked', storeNameDraftMatch);

            $('#entityMatch').prop('checked', entityMatch);
            $('#brandMatch').prop('checked', brandMatch);
            $('#periodMatch').prop('checked', periodMatch);
            $('#activityDescMatch').prop('checked', activityDescMatch);
            $('#mechanismMatch').prop('checked', mechanismMatch);
            $('#investmentMatch').prop('checked', investmentMatch);
            $('#skpSign7Match').prop('checked', skpSign7Match);
            $('#distributorMatch').prop('checked', distributorMatch);
            $('#channelMatch').prop('checked', channelMatch);
            $('#storeNameMatch').prop('checked', storeNameMatch);

        }

        blockUI.release();
        blockUIDetail.release();
        blockUIAttribute.release();
        blockUIAttachment.release();
    });

});

$('#btn_submit').on('click', function () {
    let e = document.querySelector("#btn_submit");
    validator.validate().then(async function (status) {
        if (status === "Valid") {
            e.setAttribute("data-kt-indicator", "on");
            e.disabled = !0;

            blockUI.block();
            blockUIDetail.block();
            blockUIAttribute.block();
            blockUIAttachment.block();

            let formData = new FormData($('#form_skp_validation')[0]);
            formData.append('promoId', id);
            let url = '/promo/skp-validation/update';
            $.get('/refresh-csrf').done(function(data) {
                $('meta[name="csrf-token"]').attr('content', data)
                $.ajaxSetup({
                    headers: {
                        'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                    }
                });
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
                    },
                    success: function(result, status, xhr, $form) {
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
                                window.location.href = '/promo/skp-validation';
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

                        blockUI.release();
                        blockUIDetail.release();
                        blockUIAttribute.release();
                        blockUIAttachment.release();
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
            });
        }
    });
});

$('.btn_download').on('click', function() {
    let row = $(this).val();
    if (row !== "all") {
        let attachment = $('#review_file_label_' + row);
        let url = file_host + "/assets/media/promo/" + id + "/row" + row + "/" + attachment.val();
        if (attachment.val() !== "") {
            blockUIAttachment.block();
            fetch(url).then((resp) => {
                if (resp.ok) {
                    resp.blob().then(blob => {
                        const url_blob = window.URL.createObjectURL(blob);
                        const a = document.createElement('a');
                        a.style.display = 'none';
                        a.href = url_blob;
                        a.download = $('#review_file_label_' + row).val();
                        document.body.appendChild(a);
                        a.click();
                        window.URL.revokeObjectURL(url_blob);
                        blockUIAttachment.release();
                    })
                    .catch(e => {
                        blockUIAttachment.release();
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
                    blockUIAttachment.release();
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

$('.btn_view').on('click', function () {
    let row = $(this).val();
    let attachment = $('#review_file_label_' + row);

    //Draft
    ($('#skpDraftAvail').is(":checked")) ? skpDraftAvail = "on" : skpDraftAvail = "off";
    ($('#skpDraftAvailBfrAct60').is(":checked")) ? skpDraftAvailBfrAct60 = "on" : skpDraftAvailBfrAct60 = "off";
    ($('#entityDraftMatch').is(":checked")) ? entityDraftMatch = "on" : entityDraftMatch = "off";
    ($('#brandDraftMatch').is(":checked")) ? brandDraftMatch = "on" : brandDraftMatch = "off";
    ($('#periodDraftMatch').is(":checked")) ? periodDraftMatch = "on" : periodDraftMatch = "off";
    ($('#activityDescDraftMatch').is(":checked")) ? activityDescDraftMatch = "on" : activityDescDraftMatch = "off";
    ($('#mechanismDraftMatch').is(":checked")) ? mechanismDraftMatch = "on" : mechanismDraftMatch = "off";
    ($('#investmentDraftMatch').is(":checked")) ? investmentDraftMatch = "on" : investmentDraftMatch = "off";
    ($('#distributorDraftMatch').is(":checked")) ? distributorDraftMatch = "on" : distributorDraftMatch = "off";
    ($('#channelDraftMatch').is(":checked")) ? channelDraftMatch = "on" : channelDraftMatch = "off";
    ($('#storeNameDraftMatch').is(":checked")) ? storeNameDraftMatch = "on" : storeNameDraftMatch = "off";

    //Final
    ($('#entityMatch').is(":checked")) ? entityMatch = "on" : entityMatch = "off";
    ($('#brandMatch').is(":checked")) ? brandMatch = "on" : brandMatch = "off";
    ($('#periodMatch').is(":checked")) ? periodMatch = "on" : periodMatch = "off";
    ($('#activityDescMatch').is(":checked")) ? activityDescMatch = "on" : activityDescMatch = "off";
    ($('#mechanismMatch').is(":checked")) ? mechanismMatch = "on" : mechanismMatch = "off";
    ($('#investmentMatch').is(":checked")) ? investmentMatch = "on" : investmentMatch = "off";
    ($('#skpSign7Match').is(":checked")) ? skpSign7Match = "on" : skpSign7Match = "off";
    ($('#distributorMatch').is(":checked")) ? distributorMatch = "on" : distributorMatch = "off";
    ($('#channelMatch').is(":checked")) ? channelMatch = "on" : channelMatch = "off";
    ($('#storeNameMatch').is(":checked")) ? storeNameMatch = "on" : storeNameMatch = "off";

    let flagging =  "/promo/skp-validation/flagging?id="+id+"&refId=" + promoRefId + "&r="+row+"&fileName="+attachment.val()
        + "&skpDraftAvail=" + skpDraftAvail + "&skpDraftAvailBfrAct60=" + skpDraftAvailBfrAct60 + "&entityDraftMatch="+entityDraftMatch
        + "&brandDraftMatch=" + brandDraftMatch + "&periodDraftMatch=" + periodDraftMatch + "&activityDescDraftMatch=" + activityDescDraftMatch
        + "&mechanismDraftMatch=" + mechanismDraftMatch + "&investmentDraftMatch=" + investmentDraftMatch + "&distributorDraftMatch=" + distributorDraftMatch
        + "&channelDraftMatch=" + channelDraftMatch + "&storeNameDraftMatch=" + storeNameDraftMatch
        + "&entityMatch=" + entityMatch + "&brandMatch=" + brandMatch + "&periodMatch=" + periodMatch + "&activityDescMatch=" + activityDescMatch
        + "&mechanismMatch=" + mechanismMatch + "&investmentMatch=" + investmentMatch + "&skpSign7Match=" + skpSign7Match + "&distributorMatch=" + distributorMatch
        + "&channelMatch=" + channelMatch + "&storeNameMatch=" + storeNameMatch;

    let w = window.innerWidth;
    let h = window.innerHeight;
    window.open(flagging, "promoWindow", "location=1,status=1,scrollbars=1,width=" + w + ",height=" + h);
});

const getData = (id) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/promo/skp-validation/data/id",
            type: "GET",
            data: {id:id},
            dataType: "JSON",
            success: async function (result) {
                if (!result.error) {
                    let values = result.data;

                    $('#txt_info_method').text('View ' + (values.promoHeader.refId ?? ""));
                    promoRefId = values.promoHeader.refId;

                    $('#skpStatus').val(values.skpValidations[0].skpstatus).trigger('change');
                    $('#skpNotes').val(values.skpValidations[0].skp_notes);

                    //Draft
                    $('#skpDraftAvail').prop('checked', values.skpValidations[0].skpDraftAvail);
                    $('#skpDraftAvailBfrAct60').prop('checked', values.skpValidations[0].skpDraftAvailBfrAct60);
                    $('#entityDraftMatch').prop('checked', values.skpValidations[0].entityDraft);
                    $('#brandDraftMatch').prop('checked', values.skpValidations[0].brandDraft);
                    $('#periodDraftMatch').prop('checked', values.skpValidations[0].periodDraft);
                    $('#activityDescDraftMatch').prop('checked', values.skpValidations[0].activityDescDraft);
                    $('#mechanismDraftMatch').prop('checked', values.skpValidations[0].mechanismDraft);
                    $('#investmentDraftMatch').prop('checked', values.skpValidations[0].investmentDraft);
                    $('#distributorDraftMatch').prop('checked', values.skpValidations[0].distributorDraft);
                    $('#channelDraftMatch').prop('checked', values.skpValidations[0].channelDraft);
                    $('#storeNameDraftMatch').prop('checked', values.skpValidations[0].storeNameDraft);

                    //Final
                    $('#entityMatch').prop('checked', values.skpValidations[0].entity);
                    $('#brandMatch').prop('checked', values.skpValidations[0].brand);
                    $('#periodMatch').prop('checked', values.skpValidations[0].periodMatch);
                    $('#activityDescMatch').prop('checked', values.skpValidations[0].activityDesc);
                    $('#mechanismMatch').prop('checked', values.skpValidations[0].mechanismMatch);
                    $('#investmentMatch').prop('checked', values.skpValidations[0].investmentMatch);
                    $('#skpSign7Match').prop('checked', values.skpValidations[0].skpSign7);
                    $('#distributorMatch').prop('checked', values.skpValidations[0].distributor);
                    $('#channelMatch').prop('checked', values.skpValidations[0].channel);
                    $('#storeNameMatch').prop('checked', values.skpValidations[0].storeName);

                    // header
                    $('#period').val(new Date(values.promoHeader.startPromo).getFullYear());
                    $('#promoPlanRefId').val(values.promoHeader.promoPlanRefId);
                    $('#allocationRefId').val(values.promoHeader.allocationRefId);
                    $('#allocationDesc').val(values.promoHeader.allocationDesc);

                    $('#startPromo').val(formatDateOptima(values.promoHeader.startPromo));
                    $('#endPromo').val(formatDateOptima(values.promoHeader.endPromo));

                    $('#activity').val(values.promoHeader.activityLongDesc);
                    $('#subActivityDesc').val(values.promoHeader.subActivityLongDesc);
                    $('#activityDesc').val(values.promoHeader.activityDesc);
                    $('#tsCoding').val(values.promoHeader.tsCoding);
                    $('#subCategoryDesc').val(values.promoHeader.subCategoryDesc);
                    $('#entity').val(values.promoHeader.principalName);
                    $('#distributorName').val(values.promoHeader.distributorName);
                    $('#budgetAmount').val(formatMoney(values.promoHeader.budgetAmount,0) ?? 0);
                    $('#remainingBudget').val(formatMoney(values.promoHeader.remainingBudget,0) ?? 0);
                    $('#initiatorNotes').val(values.promoHeader.initiator_notes);
                    $('#investmentType').val(values.promoHeader.investmentTypeDesc);

                    $('#baselineSales').val(formatMoney(values.promoHeader.normalSales, 0) ?? 0);
                    $('#incrementSales').val(formatMoney(values.promoHeader.incrSales, 0) ?? 0);
                    $('#investment').val(formatMoney(values.promoHeader.investment, 0) ?? 0);
                    $('#investmentBfrClose').val(formatMoney(values.promoHeader.investmentBfrClose, 0) ?? 0);
                    $('#investmentClosedBalance').val(formatMoney(values.promoHeader.investmentClosedBalance, 0) ?? 0);
                    $('#totalSales').val(formatMoney(values.promoHeader.normalSales + values.promoHeader.incrSales, 0) ?? 0);
                    $('#totalInvestment').val(formatMoney(values.promoHeader.investment, 0) ?? 0);
                    $('#roi').val(formatMoney(values.promoHeader.roi, 2) ?? 0);
                    $('#costRatio').val(formatMoney(values.promoHeader.costRatio, 2) ?? 0);
                    $('#notes_message').val(values.promoHeader.approvalNotes);

                    for (let i = 0; i < values.channels.length; i++) {
                        if (values.channels[i].flag) {
                            $('#channel').val(values.channels[i].longDesc);
                        }
                    }
                    for (let i = 0; i < values.subChannels.length; i++) {
                        if (values.subChannels[i].flag) {
                            $('#subChannel').val(values.subChannels[i].longDesc);
                        }
                    }
                    for (let i = 0; i < values.accounts.length; i++) {
                        if (values.accounts[i].flag) {
                            $('#account').val(values.accounts[i].longDesc);
                        }
                    }
                    for (let i = 0; i < values.subAccounts.length; i++) {
                        if (values.subAccounts[i].flag) {
                            $('#subAccount').val(values.subAccounts[i].longDesc);
                        }
                    }

                    //Attribute Region
                    let longDescRegion = '';
                    for (let i = 0; i < values.regions.length; i++) {
                        if (values.regions[i].flag) {
                            longDescRegion += '<span class="fw-bold text-sm-left">' + values.regions[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                        }
                    }
                    $('#card_list_region').html(longDescRegion);

                    //Attribute Brand
                    let longDescBrand = '';
                    for (let i = 0; i < values.brands.length; i++) {
                        if (values.brands[i].flag) {
                            longDescBrand += '<span class="fw-bold text-sm-left">' + values.brands[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                        }
                    }
                    $('#card_list_brand').html(longDescBrand);

                    //Attribute SKU
                    let longDescSKU = '';
                    for (let i = 0; i < values.skus.length; i++) {
                        if (values.skus[i].flag) {
                            longDescSKU += '<span class="fw-bold text-sm-left">' + values.skus[i].longDesc + '</span><div class="separator border-2 border-secondary my-2"></div>';
                        }
                    }
                    $('#card_list_sku').html(longDescSKU);

                    // Attribuate Mechanism
                    let nourut = 0;
                    dt_mechanism.clear().draw();
                    for (let i = 0; i < values.mechanisms.length; i++) {
                        nourut += 1;
                        let dMechanism = [];
                        dMechanism["no"] = nourut;
                        dMechanism["mechanism"] = values.mechanisms[i].mechanism;
                        dMechanism["notes"] = values.mechanisms[i].notes;
                        dMechanism["productId"] = values.mechanisms[i].productId;
                        dMechanism["product"] = values.mechanisms[i].product;
                        dMechanism["brandId"] = values.mechanisms[i].brandId;
                        dMechanism["brand"] = values.mechanisms[i].brand;
                        dMechanism["mechanismId"] = values.mechanisms[i].mechanismId;

                        dt_mechanism.row.add(dMechanism).draw();
                    }

                    if (values.attachments) {
                        values.attachments.forEach((item, index, arr) => {
                            if (item.docLink === 'row' + parseInt(item.docLink.replace('row', ''))) {
                                $('#review_file_label_' + parseInt(item.docLink.replace('row', ''))).val(item.fileName).attr('title', item.fileName).addClass('form-control-solid-bg');
                                $('#attachment' + parseInt(item.docLink.replace('row', ''))).attr('disabled', true);
                                $('#btn_download' + parseInt(item.docLink.replace('row', ''))).attr('disabled', false);
                                $('#btn_view' + parseInt(item.docLink.replace('row', ''))).attr('disabled', false);
                            }
                        });
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
                return reject(errorThrown);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}
