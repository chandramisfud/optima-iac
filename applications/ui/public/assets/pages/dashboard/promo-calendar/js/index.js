'use strict';

const monthNames = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
];

const elCalendar = document.getElementById('promo_calendar');
var calendar;

(function(window, document, $) {
    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
})(window, document, jQuery);

document.addEventListener('DOMContentLoaded', function () {
    let desc = $('#search_activity_desc').val();
    let url = '/dashboard/promo-calendar/list/calendar';

    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'json',
        data: {activity_desc: desc},
        async: false,
        success: function (result) {
            if (result.data) {
                let data = result.data;
                renderCalendar(data);
            }
        }
    });
});

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

    getListEntity();
    getListChannel();
    getListAccount();
    getListCategory();
});

$('#search_activity_desc').on('input', function () {
    let desc = $(this).val();
    let url = '/dashboard/promo-calendar/list/calendar';

    $('#promo_calendar').empty();

    $.ajax({
        url: url,
        method: 'GET',
        dataType: 'json',
        data: {activity_desc: desc},
        async: false,
        success: function (result) {
            if (result.data) {
                let data = result.data;
                renderCalendar(data);
            }
        }
    });
});

const renderCalendar = (data) => {
    let even = [];
    let res = [];
    for (let i = 0; i < data.length; i++) {
        let ecolor = "#FFFFFF";
        let tcolor = "#FFFFFF";
        let title = "";
        if (data[i].sts === 0) {
            ecolor = "#DDDDDD";
            tcolor = "#000000";
            title = 'IDR.' + formatMoney(data[i].planinvest, 0);
        } else if (data[i].sts === 1) {
            ecolor = "#C5D9F1";
            tcolor = "#000000";
            title = 'IDR.' + formatMoney(data[i].promoinvest, 0)
        } else if (data[i].sts === 2) {
            ecolor = "#538DD5";
            tcolor = "#000000";
            title = 'IDR.' + formatMoney(data[i].promoinvest, 0)
        } else if (data[i].sts === 3) {
            ecolor = "#000099";
            tcolor = "#ffffff";
            title = 'IDR.' + formatMoney(data[i].promoinvest, 0) + '/' + 'IDR.' + formatMoney(data[i].totalclaim, 0);
        }

        even.push({
            id: data[i].promoid,
            resourceId: data[i].planningid,
            title: title,
            start: formatDate(data[i].startpromo),
            end: formatDate(data[i].endpromo),
            textColor: tcolor,
            extendedProps: {
                warning: data[i].warningsts,
                promo: data[i].activitydesc
            }
        });
        res.push({
            "id": data[i].planningid,
            "title": data[i].activitydesc,
            "eventColor": ecolor
        });
    }
    calendar = new FullCalendar.Calendar(elCalendar, {
        height: $(window).height() - 180,
        plugins: ['interaction', 'resourceTimeline'],
        timeZone: 'UTC',
        defaultView: 'resourceTimelineYear',
        aspectRatio: 1.5,
        header: {
            left: 'prev,next',
            center: 'title',
            right: 'resourceTimelineMonth,resourceTimelineYear'
        },
        views: {
            resourceTimelineYear: {
                type: 'timeline',
                duration: { years: 1 },
                slotDuration: { month: 1 },
                columnHeaderFormat: 'MMMM YYYY',
            },
            resourceTimelineMonth: {
                type: 'timeline',
                duration: { months: 1 },
                slotDuration: { week: 1 },
            },

        },
        resourceLabelText: 'List Promo',
        resources: res,
        eventRender: function (warning) {
            if (warning.event.extendedProps.warning === 1) {
                warning.el.querySelector('.fc-title-wrap').innerHTML = '<div class="ftitle"><i class="fa fa-exclamation-circle text-yellow fs-3 me-2"></i>' + warning.event.title + '</div>';
            } else if (warning.event.extendedProps.warning === 2) {
                warning.el.querySelector('.fc-title-wrap').innerHTML = '<div class="ftitle"><i class="fa fa-exclamation-circle text-red fs-3 me-2"></i>' + warning.event.title + '</div>';
            } else {
                warning.el.querySelector('.fc-title-wrap').innerHTML = '<div class="ftitle">' + warning.event.title + '</div>';
            }
        },
        eventClick: function (info) {
            Swal.fire({
                title: "Promo Calendar",
                html: '<span>Promo: ' + info.event.extendedProps.promo + "\n" + 'Promo Investment: ' + info.event.title + '</span>',
                buttonsStyling: !1,
                confirmButtonText: "OK",
                allowOutsideClick: false,
                allowEscapeKey: false,
                customClass: {confirmButton: "btn btn-optima"}
            });
        },
        events: even,
        editable: true
    });

    calendar.render();
}

const getListEntity = () => {
    $.ajax({
        url         : "/dashboard/promo-calendar/list/entity",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            let res = result.data;
            let data = [];
            for (let j = 0, len = res.length; j < len; ++j){
                data.push({
                    id: res[j].id,
                    text: res[j].longDesc
                });
            }
            $('#filter_entity').select2({
                placeholder: "Select an Entity",
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

const getListChannel = () => {
    $.ajax({
        url         : "/dashboard/promo-calendar/list/channel",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            let res = result.data;
            let data = [];
            for (let j = 0, len = res.length; j < len; ++j){
                data.push({
                    id: res[j].id,
                    text: res[j].longDesc
                });
            }
            $('#filter_channel').select2({
                placeholder: "Select a Channel",
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

const getListAccount = () => {
    $.ajax({
        url         : "/dashboard/promo-calendar/list/account",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            let res = result.data;
            let data = [];
            for (let j = 0, len = res.length; j < len; ++j){
                data.push({
                    id: res[j].id,
                    text: res[j].longDesc
                });
            }
            $('#filter_account').select2({
                placeholder: "Select an Account",
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

const getListCategory = () => {
    $.ajax({
        url         : "/dashboard/promo-calendar/list/category",
        type        : "GET",
        dataType    : 'json',
        async       : true,
        success: function(result) {
            let res = result.data;
            let data = [];
            for (let j = 0, len = res.length; j < len; ++j){
                data.push({
                    id: res[j].id,
                    text: res[j].longDesc
                });
            }
            $('#filter_category').select2({
                placeholder: "Select a Category",
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
