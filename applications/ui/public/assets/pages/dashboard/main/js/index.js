'use strict';

// header card
const targetHeader = document.querySelector(".card_dashboard_header");
const blockUIHeader = new KTBlockUI(targetHeader, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// filter header card
const targetFilterHeader = document.querySelector(".card_filter_header");
const blockUIFilterHeader = new KTBlockUI(targetFilterHeader, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// pie chart card
const targetPromoVsBudget = document.querySelector(".card_promo_created_vs_budget");
const blockUIPromoVsBudget = new KTBlockUI(targetPromoVsBudget, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

const targetPromoApproved = document.querySelector(".card_promo_approved");
const blockUIPromoApproved = new KTBlockUI(targetPromoApproved, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

const targetPromoReconciled = document.querySelector(".card_promo_reconciled");
const blockUIPromoReconciled = new KTBlockUI(targetPromoReconciled, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

const targetPromoCreatedOnTime = document.querySelector(".card_promo_created_on_time");
const blockUIPromoCreatedOnTime = new KTBlockUI(targetPromoCreatedOnTime, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

const targetPromoApprovedOnTime = document.querySelector(".card_promo_approved_on_time");
const blockUIPromoApprovedOnTime = new KTBlockUI(targetPromoApprovedOnTime, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

const targetSubmittedClaim = document.querySelector(".card_promo_submitted_claim");
const blockUISubmittedClaim = new KTBlockUI(targetSubmittedClaim, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});
// end of pie chart card

// notif card
const targetNotif = document.querySelector(".card_dashboard_notif");
const blockUINotif = new KTBlockUI(targetNotif, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// promo creation trend card
const targetPromoCreationTrend = document.querySelector(".card_dashboard_promo_creation_trend");
const blockUIPromoCreationTrend = new KTBlockUI(targetPromoCreationTrend, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

// outstanding dn card
const targetOutstandingDN = document.querySelector(".card_dashboard_outstanding_dn");
const blockUIOutstandingDN = new KTBlockUI(targetOutstandingDN, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

const monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];

$("#card_filter").css('display', 'none');

am4core.useTheme(am4themes_animated);
am4core.options.autoSetClassName = true;

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

$(document).ready(function () {
    $('form').submit(false);

    let elYearMonth = $('#year_month');
    let ym = new Date();
    let month = ym.getMonth();
    let year = ym.getFullYear();

    elYearMonth.flatpickr({
        disableMobile: "true",
        plugins: [new monthSelectPlugin({shorthand: true, dateFormat: "Y F", altFormat: "Y F"})],
    });

    elYearMonth.val(year + " " + monthNames[month]);

    blockUIHeader.block();
    blockUIPromoVsBudget.block();
    blockUIPromoApproved.block();
    blockUIPromoReconciled.block();
    blockUIPromoCreatedOnTime.block();
    blockUIPromoApprovedOnTime.block();
    blockUISubmittedClaim.block();
    blockUINotif.block();
    blockUIPromoCreationTrend.block();
    blockUIOutstandingDN.block();

    getListChannel();
    getListAccount();
    getListDistributor();

    getDataNotifDashboard().then(function () {
        blockUINotif.release();
    });

    getListCategory().then(function () {
        let category = $("#filter_category").val();
        let category_list = [];
        for (let i=0; i < category.length; i++) {
            category_list.push(category[i]);
        }

        getDataHeaderDashboard('YTD', formatDate(new Date), ['0'],['0'], category_list).then(function () {
            blockUIHeader.release();
            blockUIPromoVsBudget.release();
            blockUIPromoApproved.release();
            blockUIPromoReconciled.release();
            blockUIPromoCreatedOnTime.release();
            blockUIPromoApprovedOnTime.release();
            blockUISubmittedClaim.release();
        });

        getDataPromoCreationTrend(formatDate(new Date), ['0'],['0'], category_list).then(function () {
            blockUIPromoCreationTrend.release();
        });

        getDataOutstandingDN(formatDate(new Date), '0','0', '0', category_list,0).then(function () {
            blockUIOutstandingDN.release();
        });
    });
});

$("#btn_show_filter").on('click', function () {
    $('#card_filter').animate({
        height: "toggle",
        opacity: "toggle"
    }, 300);
});

$('#btn_view_header').on('click', async function () {
    let filter = getFilter();

    let btn = document.getElementById('btn_view_header');
    btn.setAttribute("data-kt-indicator", "on");
    btn.disabled = !0;

    // show data dashboard header
    blockUIHeader.block();
    blockUIFilterHeader.block();
    blockUIPromoVsBudget.block();
    blockUIPromoApproved.block();
    blockUIPromoReconciled.block();
    blockUIPromoCreatedOnTime.block();
    blockUIPromoApprovedOnTime.block();
    blockUISubmittedClaim.block();
    blockUIPromoCreationTrend.block();
    blockUIOutstandingDN.block();

    let category = $("#filter_category").val();
    let category_list = [];
    for (let i=0; i < category.length; i++) {
        category_list.push(category[i]);
    }

    await getDataHeaderDashboard(filter.viewMode, filter.date, filter.channel, filter.account, category_list).then(function () {
        blockUIHeader.release();
        blockUIPromoVsBudget.release();
        blockUIPromoApproved.release();
        blockUIPromoReconciled.release();
        blockUIPromoCreatedOnTime.release();
        blockUIPromoApprovedOnTime.release();
        blockUISubmittedClaim.release();
    });

    await getDataPromoCreationTrend(filter.date, filter.channel, filter.account, category_list).then(function () {
        blockUIPromoCreationTrend.release();
    });

    await getDataOutstandingDN(filter.date, filter.channel, filter.account, filter.distributorId, category_list, filter.isPromo).then(function () {
        blockUIOutstandingDN.release();
    });

    btn.setAttribute("data-kt-indicator", "off");
    btn.disabled = !1;
    blockUIFilterHeader.release();
});

$('#btn_show_filter_outstanding_dn').on('click', function () {
    let elFilterOutstandingDN = $('#filter_outstanding_dn');
    if (elFilterOutstandingDN.hasClass('d-none')) {
        elFilterOutstandingDN.removeClass('d-none');
    } else {
        elFilterOutstandingDN.addClass('d-none');
    }
});

$('#filter_ispromo, #filter_distributor').on('change', function () {
    let category = $("#filter_category").val();
    let category_list = [];
    for (let i=0; i < category.length; i++) {
        category_list.push(category[i]);
    }

    let filter = getFilter();
    blockUIOutstandingDN.block();
    getDataOutstandingDN(filter.date, filter.channel, filter.account, filter.distributorId, category_list, filter.isPromo).then(function () {
        blockUIOutstandingDN.release();
    });
});

const getFilter = () => {
    let viewMode = $('#viewMode').val();
    let period = $('#year_month').val();
    let year, month, date;
    if (period !== "") {
        let arrYearMonth = period.split(' ');
        year = arrYearMonth[0];
        month = monthNames.indexOf(arrYearMonth[1]);
        date = formatDate(new Date(year, month + 1, 0));
    } else {
        date = formatDate(new Date());
    }

    let channel = $('#filter_channel').val();
    let account = $('#filter_account').val();

    let distributorId = $('#filter_distributor').val();
    let isPromo = $('#filter_ispromo').val();

    return {
        viewMode: viewMode,
        date: date,
        channel: channel,
        account: account,
        distributorId: distributorId,
        isPromo: isPromo
    };
}

const getListChannel = () => {
    $.ajax({
        url         : "/dashboard/main/list/channel",
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
            let elSelectChannel = $('#filter_channel');

            $.fn.select2.amd.require([
                'select2/selection/single',
                'select2/selection/placeholder',
                'select2/selection/allowClear',
                'select2/dropdown',
                'select2/dropdown/search',
                'select2/dropdown/attachBody',
                'select2/utils'
            ], function (SingleSelection, Placeholder, AllowClear, Dropdown, DropdownSearch, AttachBody, Utils) {
                let SelectionAdapter = Utils.Decorate(
                    SingleSelection,
                    Placeholder
                );

                SelectionAdapter = Utils.Decorate(
                    SelectionAdapter,
                    AllowClear
                );

                let DropdownAdapter = Utils.Decorate(
                    Utils.Decorate(
                        Dropdown,
                        DropdownSearch
                    ),
                    AttachBody
                );
                elSelectChannel.select2({
                    placeholder: 'Select a Channel',
                    selectionAdapter: SelectionAdapter,
                    dropdownAdapter: DropdownAdapter,
                    allowClear: true,
                    templateResult: function (data) {
                        if (!data.id) {
                            return data.text;
                        }

                        let $res = $('<div></div>');

                        $res.text(data.text);
                        $res.addClass('wrap');

                        return $res;
                    },
                    templateSelection: function (data) {
                        if (!data.id) {
                            return data.text;
                        }
                        let selected = (elSelectChannel.val() || []).length;
                        let total = $('option', elSelectChannel).length;
                        return "Selected " + selected + " of " + total;
                    },
                    data: data
                });

                $('[aria-controls="select2-filter_channel-container"]').addClass('form-select form-select-sm');
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

const getListCategory = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/dashboard/main/list/category",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].categoryId,
                        text: result.data[j].categoryLongDesc
                    });
                }
                let elSelectCategory = $('#filter_category');

                $.fn.select2.amd.require([
                    'select2/selection/single',
                    'select2/selection/placeholder',
                    'select2/selection/allowClear',
                    'select2/dropdown',
                    'select2/dropdown/search',
                    'select2/dropdown/attachBody',
                    'select2/utils'
                ], function (SingleSelection, Placeholder, AllowClear, Dropdown, DropdownSearch, AttachBody, Utils) {
                    let SelectionAdapter = Utils.Decorate(
                        SingleSelection,
                        Placeholder
                    );

                    SelectionAdapter = Utils.Decorate(
                        SelectionAdapter,
                        AllowClear
                    );

                    let DropdownAdapter = Utils.Decorate(
                        Utils.Decorate(
                            Dropdown,
                            DropdownSearch
                        ),
                        AttachBody
                    );
                    elSelectCategory.select2({
                        placeholder: 'Select a Category',
                        selectionAdapter: SelectionAdapter,
                        dropdownAdapter: DropdownAdapter,
                        allowClear: true,
                        templateResult: function (data) {
                            if (!data.id) {
                                return data.text;
                            }

                            let $res = $('<div></div>');

                            $res.text(data.text);
                            $res.addClass('wrap');

                            return $res;
                        },
                        templateSelection: function (data) {
                            if (!data.id) {
                                return data.text;
                            }
                            let selected = (elSelectCategory.val() || []).length;
                            let total = $('option', elSelectCategory).length;
                            return "Selected " + selected + " of " + total;
                        },
                        data: data
                    });

                    $('[aria-controls="select2-filter_category-container"]').addClass('form-select form-select-sm');
                    let category_list = [];
                    for (let i=0; i < result.data.length; i++) {
                        category_list.push(result.data[i].categoryId);
                    }
                    $('#filter_category').val(category_list).trigger('change');
                    return resolve();
                });
            },
            complete: function() {
            },
            error: function (jqXHR, textStatus, errorThrown)
            {
                console.log(jqXHR.responseText);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getListAccount = () => {
    $.ajax({
        url         : "/dashboard/main/list/account",
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
            let elSelectAccount = $('#filter_account');

            $.fn.select2.amd.require([
                'select2/selection/single',
                'select2/selection/placeholder',
                'select2/selection/allowClear',
                'select2/dropdown',
                'select2/dropdown/search',
                'select2/dropdown/attachBody',
                'select2/utils'
            ], function (SingleSelection, Placeholder, AllowClear, Dropdown, DropdownSearch, AttachBody, Utils) {
                let SelectionAdapter = Utils.Decorate(
                    SingleSelection,
                    Placeholder
                );

                SelectionAdapter = Utils.Decorate(
                    SelectionAdapter,
                    AllowClear
                );

                let DropdownAdapter = Utils.Decorate(
                    Utils.Decorate(
                        Dropdown,
                        DropdownSearch
                    ),
                    AttachBody
                );
                elSelectAccount.select2({
                    placeholder: 'Select an Account',
                    selectionAdapter: SelectionAdapter,
                    dropdownAdapter: DropdownAdapter,
                    allowClear: true,
                    templateResult: function (data) {
                        if (!data.id) {
                            return data.text;
                        }

                        let $res = $('<div></div>');

                        $res.text(data.text);
                        $res.addClass('wrap');

                        return $res;
                    },
                    templateSelection: function (data) {
                        if (!data.id) {
                            return data.text;
                        }
                        let selected = (elSelectAccount.val() || []).length;
                        let total = $('option', elSelectAccount).length;
                        return "Selected " + selected + " of " + total;
                    },
                    data: data
                });

                $('[aria-controls="select2-filter_account-container"]').addClass('form-select form-select-sm');
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

const getListDistributor = () => {
    $.ajax({
        url         : "/dashboard/main/list/distributor",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            let distributor = result.data.distributor;
            let data = [{
                id: 0,
                text: 'All'
            }];
            for (let j = 0, len = distributor.length; j < len; ++j){
                data.push({
                    id: distributor[j].id,
                    text: distributor[j].longDesc
                });
            }
            $('#filter_distributor').select2({
                placeholder: "Select a Distributor",
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

const getDataHeaderDashboard = (viewMode, period, channelId, accountId, categoryId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/dashboard/main/data",
            type: "GET",
            dataType: 'json',
            data: {viewMode:viewMode, period:period, channelId:channelId, accountId:accountId, categoryId:categoryId},
            async: true,
            success: function (result) {
                let res = result.data;
                $('#txt_budget_deployment').text(res.budget_deployed_word);
                $('#txt_promo_planning').text(res.promo_planning_word);
                $('#txt_promo_creation').text(res.promo_created_word);
                $('#txt_total_claims').text(res.total_claim_word);
                $('#txt_paid_claims').text(res.total_paid_word);

                $('#txt_promo_created').text((formatMoney(res.promoid_created, 0) ?? 0));
                $('#txt_promo_approved').text((formatMoney(res.promoid_approved, 0) ?? 0));
                $('#txt_promo_reconciled').text((formatMoney(res.promoid_reconciled, 0) ?? 0));
                $('#txt_avg_days_created_bfr_promo_start').text((formatMoney(res.avgdayscreated_bfr_promostart, 0) ?? 0));
                $('#txt_avg_promo').text((formatMoney(res.avgpromo, 0) ?? 0));
                $('#txt_claim_received').text((formatMoney(res.claim_received, 0) ?? 0));

                chartPromoCreated(res.pct_promo_created_vs_budget);
                chartPromoApproved(res.pct_promo_approved);
                chartPromoReconciled(res.pct_promo_reconciled);
                chartPromoCreatedOnTime(res.pct_promo_created_ontime);
                chartPromoApprovedOnTime(res.pct_promo_approved_ontime);
                chartPromoClaimReceived(res.pct_submitted_claim);
                return resolve();
            },
            complete: function () {

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(null);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getDataNotifDashboard = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/dashboard/main/notifications",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                let res = result.data;

                $('#txt_notif_promo_plan').text(res.promo_plan);
                $('#txt_notif_pending_promo_approval').text(res.pending_promo_approval);
                $('#txt_notif_pending_promo_recon_approval').text(res.pending_promorecon_approval);
                $('#txt_notif_promo_send_back').text(res.promo_send_back);
                $('#txt_notif_promo_send_back_recon').text(res.promo_send_back_recon);
                $('#txt_notif_dn_manual').text(res.dn_manual);
                $('#txt_notif_dn_over_budget').text(res.dn_over_budget);
                $('#txt_notif_dn_validate_by_sales').text(res.dn_validate_by_sales);

                return resolve();
            },
            complete: function () {

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(null);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getDataPromoCreationTrend = (period, channelId, accountId, categoryId) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/dashboard/main/promo-creation-trend",
            type: "GET",
            dataType: 'json',
            data: {period:period, channelId:channelId, accountId:accountId, categoryId:categoryId},
            async: true,
            success: function (result) {
                let res = result.data;

                chartPromoCreationTrend(res);

                return resolve();
            },
            complete: function () {

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(null);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const getDataOutstandingDN = (period, channelId, accountId, distributorId, categoryId, isPromo) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/dashboard/main/outstanding-dn",
            type: "GET",
            dataType: 'json',
            data: {period:period, channelId:channelId, accountId:accountId, distributorId:distributorId, categoryId:categoryId, isPromo:isPromo},
            async: true,
            success: function (result) {
                let data = result.data;

                let chart = [];
                if (data) {
                    if (data.chart.length > 0) {
                        chart = result.data.chart;
                    }
                }
                chartOutstandingDN(chart);

                return resolve();
            },
            complete: function () {

            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.responseText);
                return reject(null);
            }
        });
    }).catch((e) => {
        console.log(e);
    });
}

const chartPromoCreated = (value) => {
    let charta = am4core.create("chart_promo_created", am4charts.PieChart);
    let sisa1 = ((value > 100) ? 0 : 100-value);
    // Add data
    charta.data = [
        { "sector": "Empty", "size": sisa1 },
        { "sector": "Promo Created", "size": value },
    ];
    // Add label
    charta.innerRadius = 40;
    let labela = charta.seriesContainer.createChild(am4core.Label);
    labela.horizontalCenter = "middle";
    labela.verticalCenter = "middle";
    labela.fontSize = 26;
    labela.html = '<p class="mb-1 fw-bolder">'+value+"%</p>";
    // Add and configure Series
    let pieSeriesa = charta.series.push(new am4charts.PieSeries());
    pieSeriesa.dataFields.value = "size";
    pieSeriesa.dataFields.category = "sector";
    pieSeriesa.labels.template.disabled = true;
    pieSeriesa.ticks.template.disabled = true;

    let colorSeta = new am4core.ColorSet();
    colorSeta.list = ["#b5b5b5","#062691" ].map(function(color) {
        return new am4core.color(color);
    });
    pieSeriesa.colors = colorSeta;
}

const chartPromoApproved = (value) => {
    let charta = am4core.create("chart_promo_approved", am4charts.PieChart);
    let sisa1 = ((value > 100) ? 0 : 100-value);
    // Add data
    charta.data = [
        { "sector": "Empty", "size": sisa1 },
        { "sector": "Promo Created", "size": value },
    ];
    // Add label
    charta.innerRadius = 40;
    let labela = charta.seriesContainer.createChild(am4core.Label);
    labela.horizontalCenter = "middle";
    labela.verticalCenter = "middle";
    labela.fontSize = 26;
    labela.html = '<p class="mb-1 fw-bolder">'+value+"%</p>";
    // Add and configure Series
    let pieSeriesa = charta.series.push(new am4charts.PieSeries());
    pieSeriesa.dataFields.value = "size";
    pieSeriesa.dataFields.category = "sector";
    pieSeriesa.labels.template.disabled = true;
    pieSeriesa.ticks.template.disabled = true;

    var colorSeta = new am4core.ColorSet();
    colorSeta.list = ["#b5b5b5","#062691" ].map(function(color) {
        return new am4core.color(color);
    });
    pieSeriesa.colors = colorSeta;
}

const chartPromoReconciled = (value) => {
    let charta = am4core.create("chart_promo_reconciled", am4charts.PieChart);
    let sisa1 = ((value > 100) ? 0 : 100-value);
    // Add data
    charta.data = [
        { "sector": "Empty", "size": sisa1 },
        { "sector": "Promo Created", "size": value },
    ];
    // Add label
    charta.innerRadius = 40;
    let labela = charta.seriesContainer.createChild(am4core.Label);
    labela.horizontalCenter = "middle";
    labela.verticalCenter = "middle";
    labela.fontSize = 26;
    labela.html = '<p class="mb-1 fw-bolder">'+value+"%</p>";
    // Add and configure Series
    let pieSeriesa = charta.series.push(new am4charts.PieSeries());
    pieSeriesa.dataFields.value = "size";
    pieSeriesa.dataFields.category = "sector";
    pieSeriesa.labels.template.disabled = true;
    pieSeriesa.ticks.template.disabled = true;

    let colorSeta = new am4core.ColorSet();
    colorSeta.list = ["#b5b5b5","#062691" ].map(function(color) {
        return new am4core.color(color);
    });
    pieSeriesa.colors = colorSeta;
}

const chartPromoCreatedOnTime = (value) => {
    let charta = am4core.create("chart_promo_created_ontime", am4charts.PieChart);
    let sisa1 = ((value > 100) ? 0 : 100-value);
    // Add data
    charta.data = [
        { "sector": "Empty", "size": sisa1 },
        { "sector": "Promo Created", "size": value },
    ];
    // Add label
    charta.innerRadius = 40;
    let labela = charta.seriesContainer.createChild(am4core.Label);
    labela.horizontalCenter = "middle";
    labela.verticalCenter = "middle";
    labela.fontSize = 26;
    labela.html = '<p class="mb-1 fw-bolder">'+value+"%</p>";
    // Add and configure Series
    let pieSeriesa = charta.series.push(new am4charts.PieSeries());
    pieSeriesa.dataFields.value = "size";
    pieSeriesa.dataFields.category = "sector";
    pieSeriesa.labels.template.disabled = true;
    pieSeriesa.ticks.template.disabled = true;

    let colorSeta = new am4core.ColorSet();
    colorSeta.list = ["#b5b5b5","#007bff" ].map(function(color) {
        return new am4core.color(color);
    });
    pieSeriesa.colors = colorSeta;
}

const chartPromoApprovedOnTime = (value) => {
    let charta = am4core.create("chart_promo_approved_ontime", am4charts.PieChart);
    let sisa1 = ((value > 100) ? 0 : 100-value);
    // Add data
    charta.data = [
        { "sector": "Empty", "size": sisa1 },
        { "sector": "Promo Created", "size": value },
    ];
    // Add label
    charta.innerRadius = 40;
    let labela = charta.seriesContainer.createChild(am4core.Label);
    labela.horizontalCenter = "middle";
    labela.verticalCenter = "middle";
    labela.fontSize = 26;
    labela.html = '<p class="mb-1 fw-bolder">'+value+"%</p>";
    // Add and configure Series
    let pieSeriesa = charta.series.push(new am4charts.PieSeries());
    pieSeriesa.dataFields.value = "size";
    pieSeriesa.dataFields.category = "sector";
    pieSeriesa.labels.template.disabled = true;
    pieSeriesa.ticks.template.disabled = true;

    let colorSeta = new am4core.ColorSet();
    colorSeta.list = ["#b5b5b5","#007bff" ].map(function(color) {
        return new am4core.color(color);
    });
    pieSeriesa.colors = colorSeta;
}

const chartPromoClaimReceived = (value) => {
    let charta = am4core.create("chart_promo_claim_received", am4charts.PieChart);
    let sisa1 = ((value > 100) ? 0 : 100-value);
    // Add data
    charta.data = [
        { "sector": "Empty", "size": sisa1 },
        { "sector": "Promo Created", "size": value },
    ];
    // Add label
    charta.innerRadius = 40;
    let labela = charta.seriesContainer.createChild(am4core.Label);
    labela.horizontalCenter = "middle";
    labela.verticalCenter = "middle";
    labela.fontSize = 26;
    labela.html = '<p class="mb-1 fw-bolder">'+value+"%</p>";
    // Add and configure Series
    let pieSeriesa = charta.series.push(new am4charts.PieSeries());
    pieSeriesa.dataFields.value = "size";
    pieSeriesa.dataFields.category = "sector";
    pieSeriesa.labels.template.disabled = true;
    pieSeriesa.ticks.template.disabled = true;

    let colorSeta = new am4core.ColorSet();
    colorSeta.list = ["#b5b5b5","#007bff" ].map(function(color) {
        return new am4core.color(color);
    });
    pieSeriesa.colors = colorSeta;
}

const chartPromoCreationTrend = (value) => {
    let chart = am4core.create("chartdiv_promo_creation_trend", am4charts.XYChart);
    chart.paddingRight = 10;
    let legendContainer = am4core.create("legenddivtrend", am4core.Container);
    legendContainer.width = am4core.percent(100);
    legendContainer.height = am4core.percent(100);
    chart.legend = new am4charts.Legend();
    chart.legend.parent = legendContainer;
    chart.legend.useDefaultMarker = false;
    chart.legend.labels.template.text = "[bold]{name}[/]";
    chart.legend.fill = am4core.color("#b3a2c7","#007bff");
    chart.legend.position = "top";
    chart.legend.contentAlign = "right";
    // Add data
    chart.data = value ;

    // Create axis
    let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "period";
    categoryAxis.renderer.minGridDistance = 20;
    categoryAxis.renderer.labels.template.fontSize = 10;
    categoryAxis.renderer.labels.template.rotation = 50;
    categoryAxis.renderer.grid.template.stroke = "#fbfbfb";
    // Create value axis
    let valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    //valueAxis.renderer.labels.template.dy = -10;
    chart.maskBullets = false;
    valueAxis.min = 0;
    valueAxis.max = 1;
    chart.numberFormatter.numberFormat = "#.#%";
    valueAxis.renderer.grid.template.stroke = "#fbfbfb";
    // valueAxis.renderer.minGridDistance = 20;

    // Create series
    let series1 = chart.series.push(new am4charts.LineSeries());
    series1.dataFields.valueY = "onTime";
    series1.dataFields.categoryX = "period";
    series1.stroke = am4core.color("#b3a2c7");
    series1.strokeWidth = 1;
    series1.tensionX = 1;
    series1.name = "On Time";
    let shadow1 = series1.filters.push(new am4core.DropShadowFilter);
    shadow1.dx = 1;
    shadow1.dy = 1;
    shadow1.blur = 10;
    let bullet1 = series1.bullets.push(new am4charts.CircleBullet());
    bullet1.zIndex = 1000;
    bullet1.fill = am4core.color("#b3a2c7");
    bullet1.tooltipText = "On Time: [bold]{valueY}[/]";
    bullet1.circle.radius = 3;

    let series2 = chart.series.push(new am4charts.LineSeries());
    series2.dataFields.valueY = "late";
    series2.dataFields.categoryX = "period";
    series2.stroke = am4core.color("#007bff");
    series2.strokeWidth = 1;
    series2.tensionX = 1;
    series2.name = "Late"
    let shadow2 = series2.filters.push(new am4core.DropShadowFilter);
    shadow2.dx = 1;
    shadow2.dy = 1;
    shadow2.blur = 10;
    let bullet2 = series2.bullets.push(new am4charts.CircleBullet());
    bullet2.fill = am4core.color("#007bff");
    bullet2.tooltipText = "Late: [bold]{valueY}[/]";
    bullet2.circle.radius = 3;
}

const chartOutstandingDN = (ChartsDataProvider) => {
    let chart = am4core.create("chartdiv_outstanding", am4charts.XYChart3D);
    chart.depth = 10;
    let legendContainer = am4core.create("legenddivdn", am4core.Container);
    legendContainer.width = am4core.percent(100);
    legendContainer.height = am4core.percent(100);
    chart.legend = new am4charts.Legend();
    chart.legend.parent = legendContainer;
    chart.legend.useDefaultMarker = false;

    chart.legend.position = "top";
    chart.legend.marginTop = 0;
    chart.legend.contentAlign = "right";
    let marker = chart.legend.markers.template.children.getIndex(0);
    marker.cornerRadius(15, 15, 15, 15);
    marker.width = 5;
    marker.strokeWidth = 0;
    marker.strokeOpacity = 0;

    // Add data
    chart.data = ChartsDataProvider;

    // Create axis
    let categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
    categoryAxis.dataFields.category = "days";
    categoryAxis.renderer.minGridDistance = 30;
    categoryAxis.renderer.cellStartLocation = 0.3;
    categoryAxis.renderer.cellEndLocation = 0.7;
    categoryAxis.renderer.grid.template.stroke = "#fbfbfb";

    let  valueAxis = chart.yAxes.push(new am4charts.ValueAxis());
    valueAxis.renderer.grid.template.stroke = "#fbfbfb";

    // Display numbers as percent
    chart.numberFormatter.numberFormat = "#,###";
    chart.numberFormatter.bigNumberPrefixes = [

        {"number": 1e+6, "suffix": "Mio" },
        { "number": 1e+9, "suffix": " Bio" }
    ];

    // Create series
    let seriesNIS = chart.series.push(new am4charts.ColumnSeries3D());
    seriesNIS.dataFields.valueY = "nis";
    seriesNIS.dataFields.categoryX = "days";
    seriesNIS.columns.template.fill = am4core.color("#030f57");
    seriesNIS.name = "NIS";
    seriesNIS.tooltipText = "{name}: [bold]{valueY.formatNumber('#,###')}[/]";

    let seriesSGM = chart.series.push(new am4charts.ColumnSeries3D());
    seriesSGM.dataFields.valueY = "sgm";
    seriesSGM.dataFields.categoryX = "days";
    seriesSGM.columns.template.fill = am4core.color("#007bff");
    seriesSGM.name = "SH";
    seriesSGM.tooltipText = "{name}: [bold]{valueY.formatNumber('#,###')}[/]";

    let seriesNMN = chart.series.push(new am4charts.ColumnSeries3D());
    seriesNMN.dataFields.valueY = "nmn";
    seriesNMN.dataFields.categoryX = "days";
    seriesNMN.columns.template.fill = am4core.color("#b3a2c7");
    seriesNMN.name = "NMN";
    seriesNMN.tooltipText = "{name}: [bold]{valueY.formatNumber('#,###')}[/]";

    // Add cursor
    chart.cursor = new am4charts.XYCursor();
}

// initialize chart
chartPromoCreated(0);
chartPromoApproved(0);
chartPromoReconciled(0);
chartPromoCreatedOnTime(0);
chartPromoApprovedOnTime(0);
chartPromoClaimReceived(0);
chartPromoCreationTrend([]);
chartOutstandingDN([]);
