'use strict';
var groupColumn = 3;
var dataEmailSend = [];
var defaultRegular = [];
$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    getListUserGroup();
    $('#filter_month_start').val('1').trigger('change');
    dt_profile = $('#dt_profile').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        processing: true,
        serverSide: false,
        searching: true,
        paging: false,
        scrollCollapse: true,
        ordering: false,
        scrollY: "55vh",
        deferRender: true,
        columnDefs: [
            {
                targets: 0,
                title: 'Email',
                data: 'email',
                width: 150,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data != null && full.statusName === "inactive") {
                        return data + '&nbsp;<i tabindex="0" data-toggle="tooltip" title="User is Inactive" class="fas fa-exclamation-circle" style="color:blue" /aria-hidden="true"/></i>';
                    } else {
                        return data;
                    }
                }
            },
            {
                targets: 1,
                title: 'Username',
                data: 'userName',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'User Group',
                data: 'userGroupName',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Status',
                data: 'statusName',
                visible: false,
                searchable: false,
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {

        },
        drawCallback: function( settings, json ) {

        },
    });

    dt_send_email = $('#dt_send_email').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        processing: true,
        serverSide: false,
        searching: true,
        paging: false,
        scrollCollapse: true,
        scrollY: "55vh",
        deferRender: true,
        columnDefs: [
            { visible: true, targets: groupColumn },
            {
                targets: 0,
                title: 'Email',
                data: 'email',
                width: 150,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data != null && full.statusName == "inactive") {
                        return data + '&nbsp;<i tabindex="0" data-toggle="tooltip" title="User is Inactive" class="fas fa-exclamation-circle" style="color:blue" /aria-hidden="true"/></i>';
                    } else {
                        return data;
                    }
                    // return data.toLocaleString(undefined, {minimumFractionDigits: 0, maximumFractionDigits: 0});
                }
            },
            {
                targets: 1,
                title: 'Username',
                data: 'userName',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 2,
                title: 'User Group',
                data: 'userGroupName',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'Status',
                data: 'statusName',
                visible: false,
                searchable: false,
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

$('#dt_profile_search').on('keyup', function () {
    dt_profile.search(this.value).draw();
});

$('#btn_send').on('click', function() {
    let year = $('#filter_year').val();

    let rowdata = dt_send_email
        .rows()
        .data();

    let dataSend = []
    let dataEmailSave = []
    let objEmail = {}
    $.each(rowdata, function (index, rowId) {
        dataSend.push(rowId.email)
        objEmail = {}
        objEmail.email = rowId.email
        objEmail.userName = rowId.userName
        objEmail.userGroupName = rowId.userGroupName
        objEmail.statusName = 'active'
        dataEmailSave.push(objEmail)
    });

    if (dataSend.length == 0) {
        Swal.fire({
            text: "Select one or more email",
            icon: "warning",
            buttonsStyling: !1,
            confirmButtonText: "Confirm",
            customClass: {confirmButton: "btn btn-optima"},
            closeOnConfirm: false,
            showLoaderOnConfirm: false,
            closeOnClickOutside: false,
            closeOnEsc: false,
            allowOutsideClick: false,
        });
    } else {
        let emailSend = JSON.stringify(dataSend);

        let strMonthStart = $('#filter_month_start').find(':selected')[0].innerHTML;
        let strMonthEnd = $('#filter_month_end').find(':selected')[0].innerHTML;

        let monthStart = $('#filter_month_start').val();
        let monthEnd = $('#filter_month_end').val();
        let e = document.querySelector("#btn_send");

        let emailSave = JSON.stringify(dataEmailSave);
        $.ajax({
            url         : '/fin-rpt/listing-promo-reporting/send-email',
            type        : 'POST',
            async       : true,
            data        : {year:year, month_start: monthStart, month_end: monthEnd, strMonthStart: strMonthStart, strMonthEnd: strMonthEnd ,emailSend: emailSend, emailSave: emailSave},
            dataType    : "JSON",
            beforeSend: function() {
                e.setAttribute("data-kt-indicator", "on");
                e.disabled = !0;
            },
            success: function(result, status, xhr, $form) {
                if (!result.error) {
                    $('#modal-send-email-report').modal('hide');
                    Swal.fire({
                        text: result.message,
                        icon: "success",
                        confirmButtonText: "OK",
                        customClass: {confirmButton: "btn btn-optima"}
                    }).then(function (result) {
                        $('#dt_profile_search').val('');
                        $('#dt_send_email_search').val('');
                        dt_send_email.clear().draw();
                    });
                } else {
                    $('#modal-send-email-report').modal('hide');
                    Swal.fire({
                        text: result.message,
                        icon: "warning",
                        confirmButtonText: "Confirm",
                        allowOutsideClick: false,
                        customClass: {confirmButton: "btn btn-optima"},
                    });
                }
            },
            complete: function() {
                e.setAttribute("data-kt-indicator", "off");
                e.disabled = !1;
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log(jqXHR.message)
                Swal.fire({
                    title: swalTitle,
                    text: "Failed to save data, an error occurred in the process",
                    icon: "error",
                    confirmButtonText: "OK",
                    customClass: {confirmButton: "btn btn-optima"}
                });
            }
        });
    }
});

$('#filter_groupuser').on('change', async function () {
    let groupuser = $(this).val();
    if (groupuser == "" || groupuser == null) { groupuser = "all" };

    let url = "/fin-rpt/listing-promo-reporting/get-data/user-list?usergroupid=" + groupuser + "&userlevel=" + 0 + "&status=" + 0;
    dt_profile.clear().draw()
    dt_profile.ajax.url(url).load();
});

$('#modal-send-email-report').on('hidden.bs.modal', function () {
    $('#modal-send-email-report').val('')
});

$('#dt_profile').on( 'dblclick', 'tr', function () {
    let data = dt_profile.row(this).data();

    let idx = dt_send_email
        .columns(0)
        .data()
        .eq(0)
        .indexOf(data.email);

    if (idx === -1 && data.email !== null && data.statusName === 'active' && data.userGroupName !== null) {
        let dprofile = []
        dprofile["email"] = data.email;
        dprofile["userName"] = data.userName;
        dprofile["userGroupName"] = data.userGroupName;
        dprofile["statusName"] = data.statusName;
        dt_send_email.row.add(dprofile).draw();


    }

    let dprofiles = []
    dprofiles.push({
        id: data.userGroupName,
        text: data.userGroupName
    });

    $('#filter_groupuser_grouping').select2({
        dropdownParent: $("#modal-send-email-report"),
        placeholder: "Select an User Group Menu",
        allowClear: true,
        data: dprofiles
    });

    $('#filter_groupuser_grouping').val('').trigger('change');
});

$('#dt_send_email').on( 'dblclick', 'tr', function () {
    $('#btn_del').html("Remove All");
    var tr = $(this).closest('tr');


    let trindex = dt_send_email.row(tr).index();
    dt_send_email.row(trindex).remove().draw();

});

const getDataRegular = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/listing-promo-reporting/get-data/user-regular",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                configurationId = result.data.id;
                if (result.data) {
                    dt_send_email.clear().draw();
                    dt_send_email.rows.add(result.data).draw();
                }


                let dprofiles = [];
                for (let i = 0; i < result.data.length; i++) {
                    defaultRegular.push(result.data[i]);
                    dprofiles.push({
                        id: result.data[i].userGroupName,
                        text: result.data[i].userGroupName
                    });
                }

                $('#filter_groupuser_grouping').select2({
                    placeholder: "Select an User Group Menu",
                    width: '100%',
                    data: dprofiles
                });

                $('#filter_groupuser_grouping').val('').trigger('change');
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

const getListUserGroup = (usergroupid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/listing-promo-reporting/list/usergroup",
            type        : "GET",
            dataType    : 'json',
            data        : {usergroupid: usergroupid},
            async       : true,
            success: function(result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].usergroupid,
                        text: result.data[j].usergroupname,
                    });
                }
                $('#filter_groupuser').select2({
                    placeholder: "Select an User Group Menu",
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

$('#btn_add_all').on('click', function() {
    $('#btn_del').html("Remove All");

    dt_profile.rows().every(function () {

        let datas = dt_profile.row(this).data();

        var idx = dt_send_email
            .columns(0)
            .data()
            .eq(0)
            .indexOf(datas.email);

        if (idx === -1 && datas.email !== null && datas.statusName === 'active' && datas.userGroupName !== null) {
            dt_send_email.row.add(this.data()).draw();
            dataEmailSend.push(this.data());
            let dprofiles = []
            dprofiles.push({
                id: datas.userGroupName,
                text: datas.userGroupName
            })
            $('#filter_groupuser_grouping').select2({
                dropdownParent: $("#modal-send-email-report"),
                placeholder: "Select an User Group Menu",
                allowClear: true,
                data: dprofiles
            });
            $('#filter_groupuser_grouping').val('').trigger('change');
        }
    });
});

$('#btn_remove').on('click', function() {
    var del_group = $('#filter_groupuser_grouping').val();
    if ($('#btn_del').html() === "Remove All") {
        dataEmailSend = []
        dt_send_email.rows().remove().draw();
        $('#filter_groupuser_grouping').empty();
    } else {
        dt_send_email.rows(function (idx, data, node) {
            return data.usergroupname === del_group
        }).remove().draw();
        $('#filter_groupuser_grouping').val(null).trigger('change');
        $("#filter_groupuser_grouping option[value='" + del_group + "']").remove();
        $('#btn_del').html("Remove All");
    }
});
