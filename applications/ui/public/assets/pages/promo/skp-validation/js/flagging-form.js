'use strict';
let skpDraftAvail = 0, skpDraftAvailBfrAct60 = 0, entityDraftMatch = 0, brandDraftMatch = 0, periodDraftMatch = 0, activityDescDraftMatch = 0,
    mechanismDraftMatch = 0, investmentDraftMatch = 0, distributorDraftMatch = 0, channelDraftMatch = 0, storeNameDraftMatch = 0;
let entityMatch = 0, brandMatch = 0, periodMatch = 0, activityDescMatch = 0, mechanismMatch = 0, investmentMatch = 0,
    skpSign7Match = 0, distributorMatch = 0, channelMatch = 0, storeNameMatch = 0;

let id, refId, row, fileName, categoryShortDescEnc;

$(document).ready(function () {
    let url_str = new URL(window.location.href);
    id = url_str.searchParams.get("id");
    categoryShortDescEnc = url_str.searchParams.get("c");
    row = url_str.searchParams.get("r");
    fileName = url_str.searchParams.get("fileName");
    refId = url_str.searchParams.get("refId");

    let url = file_host + "/assets/media/promo/" + id + "/row" + row + "/" + fileName;
    $('#txt_info_method').html('Promo ID ' + refId)
    $('#iframe_file').prop('src', url)

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

    if (skpDraftAvail && skpDraftAvailBfrAct60 && entityDraftMatch && brandDraftMatch && periodDraftMatch && activityDescDraftMatch &&
        mechanismDraftMatch && investmentDraftMatch && distributorDraftMatch && channelDraftMatch && storeNameDraftMatch) {
        $('#draft_all').prop('checked', true);
    } else {
        $('#draft_all').prop('checked', false);
    }
    $('#skpDraftAvail_preview').prop('checked', skpDraftAvail);
    $('#skpDraftAvailBfrAct60_preview').prop('checked', skpDraftAvailBfrAct60);
    $('#entityDraftMatch_preview').prop('checked', entityDraftMatch);
    $('#brandDraftMatch_preview').prop('checked', brandDraftMatch);
    $('#periodDraftMatch_preview').prop('checked', periodDraftMatch);
    $('#activityDescDraftMatch_preview').prop('checked', activityDescDraftMatch);
    $('#mechanismDraftMatch_preview').prop('checked', mechanismDraftMatch);
    $('#investmentDraftMatch_preview').prop('checked', investmentDraftMatch);
    $('#distributorDraftMatch_preview').prop('checked', distributorDraftMatch);
    $('#channelDraftMatch_preview').prop('checked', channelDraftMatch);
    $('#storeNameDraftMatch_preview').prop('checked', storeNameDraftMatch);

    if (entityMatch && brandMatch && periodMatch && activityDescMatch && mechanismMatch &&
        investmentMatch && skpSign7Match && distributorMatch && channelMatch && storeNameMatch) {
        $('#final_all').prop('checked', true);
    } else {
        $('#final_all').prop('checked', false);
    }
    $('#entityMatch_preview').prop('checked', entityMatch);
    $('#brandMatch_preview').prop('checked', brandMatch);
    $('#periodMatch_preview').prop('checked', periodMatch);
    $('#activityDescMatch_preview').prop('checked', activityDescMatch);
    $('#mechanismMatch_preview').prop('checked', mechanismMatch);
    $('#investmentMatch_preview').prop('checked', investmentMatch);
    $('#skpSign7Match_preview').prop('checked', skpSign7Match);
    $('#distributorMatch_preview').prop('checked', distributorMatch);
    $('#channelMatch_preview').prop('checked', channelMatch);
    $('#storeNameMatch_preview').prop('checked', storeNameMatch);
});

$('#skpDraftAvail_preview, #skpDraftAvailBfrAct60_preview, #entityDraftMatch_preview, #brandDraftMatch_preview, #periodDraftMatch_preview, #activityDescDraftMatch_preview, ' +
    '#mechanismDraftMatch_preview, #investmentDraftMatch_preview, #distributorDraftMatch_preview, #channelDraftMatch_preview, #storeNameDraftMatch_preview').on('change', function () {
    ($('#skpDraftAvail_preview').is(":checked")) ? skpDraftAvail = 1 : skpDraftAvail = 0;
    ($('#skpDraftAvailBfrAct60_preview').is(":checked")) ? skpDraftAvailBfrAct60 = 1 : skpDraftAvailBfrAct60 = 0;
    ($('#entityDraftMatch_preview').is(":checked")) ? entityDraftMatch = 1 : entityDraftMatch = 0;
    ($('#brandDraftMatch_preview').is(":checked")) ? brandDraftMatch = 1 : brandDraftMatch = 0;
    ($('#periodDraftMatch_preview').is(":checked")) ? periodDraftMatch = 1 : periodDraftMatch = 0;
    ($('#activityDescDraftMatch_preview').is(":checked")) ? activityDescDraftMatch = 1 : activityDescDraftMatch = 0;
    ($('#mechanismDraftMatch_preview').is(":checked")) ? mechanismDraftMatch = 1 : mechanismDraftMatch = 0;
    ($('#investmentDraftMatch_preview').is(":checked")) ? investmentDraftMatch = 1 : investmentDraftMatch = 0;
    ($('#distributorDraftMatch_preview').is(":checked")) ? distributorDraftMatch = 1 : distributorDraftMatch = 0;
    ($('#channelDraftMatch_preview').is(":checked")) ? channelDraftMatch = 1 : channelDraftMatch = 0;
    ($('#storeNameDraftMatch_preview').is(":checked")) ? storeNameDraftMatch = 1 : storeNameDraftMatch = 0;

    if (skpDraftAvail && skpDraftAvailBfrAct60 && entityDraftMatch && brandDraftMatch && periodDraftMatch && activityDescDraftMatch &&
        mechanismDraftMatch && investmentDraftMatch && distributorDraftMatch && channelDraftMatch && storeNameDraftMatch) {
        $('#draft_all').prop('checked', true);
    } else {
        $('#draft_all').prop('checked', false);
    }
});

$('#entityMatch_preview, #brandMatch_preview, #periodMatch_preview, #activityDescMatch_preview, #mechanismMatch_preview, #investmentMatch_preview, ' +
    '#skpSign7Match_preview, #distributorMatch_preview, #channelMatch_preview, #storeNameMatch_preview').on('change', function () {
    ($('#entityMatch_preview').is(":checked")) ? entityMatch = 1 : entityMatch = 0;
    ($('#brandMatch_preview').is(":checked")) ? brandMatch = 1 : brandMatch = 0;
    ($('#periodMatch_preview').is(":checked")) ? periodMatch = 1 : periodMatch = 0;
    ($('#activityDescMatch_preview').is(":checked")) ? activityDescMatch = 1 : activityDescMatch = 0;
    ($('#mechanismMatch_preview').is(":checked")) ? mechanismMatch = 1 : mechanismMatch = 0;
    ($('#investmentMatch_preview').is(":checked")) ? investmentMatch = 1 : investmentMatch = 0;
    ($('#skpSign7Match_preview').is(":checked")) ? skpSign7Match = 1 : skpSign7Match = 0;
    ($('#distributorMatch_preview').is(":checked")) ? distributorMatch = 1 : distributorMatch = 0;
    ($('#channelMatch_preview').is(":checked")) ? channelMatch = 1 : channelMatch = 0;
    ($('#storeNameMatch_preview').is(":checked")) ? storeNameMatch = 1 : storeNameMatch = 0;

    if (entityMatch && brandMatch && periodMatch && activityDescMatch && mechanismMatch && investmentMatch &&
        skpSign7Match && distributorMatch && channelMatch && storeNameMatch) {
        $('#final_all').prop('checked', true);
    } else {
        $('#final_all').prop('checked', false);
    }
});

$('#draft_all').on('change', function () {
    if(this.checked) {
        $('#skpDraftAvail_preview').prop('checked', 1);
        $('#skpDraftAvailBfrAct60_preview').prop('checked', 1);
        $('#entityDraftMatch_preview').prop('checked', 1);
        $('#brandDraftMatch_preview').prop('checked', 1);
        $('#periodDraftMatch_preview').prop('checked', 1);
        $('#activityDescDraftMatch_preview').prop('checked', 1);
        $('#mechanismDraftMatch_preview').prop('checked', 1);
        $('#investmentDraftMatch_preview').prop('checked', 1);
        $('#distributorDraftMatch_preview').prop('checked', 1);
        $('#channelDraftMatch_preview').prop('checked', 1);
        $('#storeNameDraftMatch_preview').prop('checked', 1);
    } else {
        $('#skpDraftAvail_preview').prop('checked', 0);
        $('#skpDraftAvailBfrAct60_preview').prop('checked', 0);
        $('#entityDraftMatch_preview').prop('checked', 0);
        $('#brandDraftMatch_preview').prop('checked', 0);
        $('#periodDraftMatch_preview').prop('checked', 0);
        $('#activityDescDraftMatch_preview').prop('checked', 0);
        $('#mechanismDraftMatch_preview').prop('checked', 0);
        $('#investmentDraftMatch_preview').prop('checked', 0);
        $('#distributorDraftMatch_preview').prop('checked', 0);
        $('#channelDraftMatch_preview').prop('checked', 0);
        $('#storeNameDraftMatch_preview').prop('checked', 0);
    }
});

$('#final_all').on('change', function () {
    if(this.checked) {
        $('#entityMatch_preview').prop('checked', 1);
        $('#brandMatch_preview').prop('checked', 1);
        $('#periodMatch_preview').prop('checked', 1);
        $('#activityDescMatch_preview').prop('checked', 1);
        $('#mechanismMatch_preview').prop('checked', 1);
        $('#investmentMatch_preview').prop('checked', 1);
        $('#skpSign7Match_preview').prop('checked', 1);
        $('#distributorMatch_preview').prop('checked', 1);
        $('#channelMatch_preview').prop('checked', 1);
        $('#storeNameMatch_preview').prop('checked', 1);
    } else {
        $('#entityMatch_preview').prop('checked', 0);
        $('#brandMatch_preview').prop('checked', 0);
        $('#periodMatch_preview').prop('checked', 0);
        $('#activityDescMatch_preview').prop('checked', 0);
        $('#mechanismMatch_preview').prop('checked', 0);
        $('#investmentMatch_preview').prop('checked', 0);
        $('#skpSign7Match_preview').prop('checked', 0);
        $('#distributorMatch_preview').prop('checked', 0);
        $('#channelMatch_preview').prop('checked', 0);
        $('#storeNameMatch_preview').prop('checked', 0);
    }
});

$('#btn_back').on('click', function () {
    //Draft
    ($('#skpDraftAvail_preview').is(":checked")) ? skpDraftAvail = "on" : skpDraftAvail = "off";
    ($('#skpDraftAvailBfrAct60_preview').is(":checked")) ? skpDraftAvailBfrAct60 = "on" : skpDraftAvailBfrAct60 = "off";
    ($('#entityDraftMatch_preview').is(":checked")) ? entityDraftMatch = "on" : entityDraftMatch = "off";
    ($('#brandDraftMatch_preview').is(":checked")) ? brandDraftMatch = "on" : brandDraftMatch = "off";
    ($('#periodDraftMatch_preview').is(":checked")) ? periodDraftMatch = "on" : periodDraftMatch = "off";
    ($('#activityDescDraftMatch_preview').is(":checked")) ? activityDescDraftMatch = "on" : activityDescDraftMatch = "off";
    ($('#mechanismDraftMatch_preview').is(":checked")) ? mechanismDraftMatch = "on" : mechanismDraftMatch = "off";
    ($('#investmentDraftMatch_preview').is(":checked")) ? investmentDraftMatch = "on" : investmentDraftMatch = "off";
    ($('#distributorDraftMatch_preview').is(":checked")) ? distributorDraftMatch = "on" : distributorDraftMatch = "off";
    ($('#channelDraftMatch_preview').is(":checked")) ? channelDraftMatch = "on" : channelDraftMatch = "off";
    ($('#storeNameDraftMatch_preview').is(":checked")) ? storeNameDraftMatch = "on" : storeNameDraftMatch = "off";

    //Final
    ($('#entityMatch_preview').is(":checked")) ? entityMatch = "on" : entityMatch = "off";
    ($('#brandMatch_preview').is(":checked")) ? brandMatch = "on" : brandMatch = "off";
    ($('#periodMatch_preview').is(":checked")) ? periodMatch = "on" : periodMatch = "off";
    ($('#activityDescMatch_preview').is(":checked")) ? activityDescMatch = "on" : activityDescMatch = "off";
    ($('#mechanismMatch_preview').is(":checked")) ? mechanismMatch = "on" : mechanismMatch = "off";
    ($('#investmentMatch_preview').is(":checked")) ? investmentMatch = "on" : investmentMatch = "off";
    ($('#skpSign7Match_preview').is(":checked")) ? skpSign7Match = "on" : skpSign7Match = "off";
    ($('#distributorMatch_preview').is(":checked")) ? distributorMatch = "on" : distributorMatch = "off";
    ($('#channelMatch_preview').is(":checked")) ? channelMatch = "on" : channelMatch = "off";
    ($('#storeNameMatch_preview').is(":checked")) ? storeNameMatch = "on" : storeNameMatch = "off";

    let url =  "/promo/skp-validation/form?method=validate&id=" + id
        + "&skpDraftAvail=" + skpDraftAvail + "&skpDraftAvailBfrAct60=" + skpDraftAvailBfrAct60 + "&entityDraftMatch="+entityDraftMatch
        + "&brandDraftMatch=" + brandDraftMatch + "&periodDraftMatch=" + periodDraftMatch + "&activityDescDraftMatch=" + activityDescDraftMatch
        + "&mechanismDraftMatch=" + mechanismDraftMatch + "&investmentDraftMatch=" + investmentDraftMatch + "&distributorDraftMatch=" + distributorDraftMatch
        + "&channelDraftMatch=" + channelDraftMatch + "&storeNameDraftMatch=" + storeNameDraftMatch
        + "&entityMatch=" + entityMatch + "&brandMatch=" + brandMatch + "&periodMatch=" + periodMatch + "&activityDescMatch=" + activityDescMatch
        + "&mechanismMatch=" + mechanismMatch + "&investmentMatch=" + investmentMatch + "&skpSign7Match=" + skpSign7Match + "&distributorMatch=" + distributorMatch
        + "&channelMatch=" + channelMatch + "&storeNameMatch=" + storeNameMatch + "&c=" + categoryShortDescEnc;

    window.opener.document.location.href = url;
    window.close();
});
