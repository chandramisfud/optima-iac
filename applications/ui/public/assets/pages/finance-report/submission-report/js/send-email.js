'use strict';
var groupColumn = 3;
var dataEmailSend = [];

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });
    dt_profile = $('#dt_profile').DataTable({
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
            {
                targets: 0,
                data: 'id',
                width: 80,
                title: 'Email',
                visible: false,
                searchable: false,
                className: 'text-nowrap align-middle text-start',
            },
            {
                targets: 1,
                title: 'Email',
                data: 'email',
                width: 150,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data != null && full.statusname == "Inactive") {
                        return data + '&nbsp;<i tabindex="0" data-toggle="tooltip" title="User is Inactive" class="fas fa-exclamation-circle" style="color:blue" /aria-hidden="true"/></i>';
                    } else {
                        return data;
                    }
                    // return data.toLocaleString(undefined, {minimumFractionDigits: 0, maximumFractionDigits: 0});
                }
            },
            {
                targets: 2,
                title: 'Username',
                data: 'username',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'User Group',
                data: 'usergroupname',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Status',
                data: 'statusname',
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
                data: 'id',
                width: 80,
                title: 'Email',
                visible: false,
                searchable: false,
                className: 'text-nowrap align-middle text-start',
            },
            {
                targets: 1,
                title: 'Email',
                data: 'email',
                width: 150,
                className: 'text-nowrap align-middle',
                render: function (data, type, full, meta) {
                    if (data != null && full.statusname == "Inactive") {
                        return data + '&nbsp;<i tabindex="0" data-toggle="tooltip" title="User is Inactive" class="fas fa-exclamation-circle" style="color:blue" /aria-hidden="true"/></i>';
                    } else {
                        return data;
                    }
                    // return data.toLocaleString(undefined, {minimumFractionDigits: 0, maximumFractionDigits: 0});
                }
            },
            {
                targets: 2,
                title: 'Username',
                data: 'username',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 3,
                title: 'User Group',
                data: 'usergroupname',
                width: 100,
                className: 'text-nowrap align-middle',
            },
            {
                targets: 4,
                title: 'Status',
                data: 'statusname',
                visible: false,
                searchable: false,
                className: 'text-nowrap align-middle',
            },
        ],
        initComplete: function( settings, json ) {
            $('body').find('.dataTables_scrollBody').addClass("scrollbar");
            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
        },
        drawCallback: function( settings, json ) {
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;

            api.column(groupColumn, { page: 'current' }).data().each(function (group, i) {
                if (last !== group) {
                    $(rows).eq(i).before(
                        '<tr  style="background-color:#94b5e8"><td id="group" colspan="5">' + group + '</td></tr>'
                    );

                    last = group;
                }
            });
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
    let year = $('#filter_period').val();
    let distributor = $('#filter_distributor').val();
    let budgetParent = $('#filter_budgetparent').val();
    let entity = $('#filter_principal').val();
    let channel = $('#filter_channel').val();
    if (distributor == "" || distributor == null) { distributor = "0"; }
    if (budgetParent == "" || budgetParent == null) { budgetParent = "0"; }
    if (entity == "" || entity == null) { entity = "0"; }
    if (channel == "" || channel == null) { channel = "0"; }

    let rowdata = dt_send_email
        .rows()
        .data();

    let dataSend = []
    $.each(rowdata, function (index, rowId) {
        dataSend.push(rowId.email)
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
        let e = document.querySelector("#btn_send");

        $.ajax({
            url         : '/fin-rpt/submission/send-email',
            type        : 'GET',
            async       : true,
            data        : {period:year, entity: entity, distributor: distributor, channel: channel, emailSend: emailSend},
            dataType    : "JSON",
            beforeSend: function() {
                e.setAttribute("data-kt-indicator", "on");
                e.disabled = !0;
            },
            success: function(result, status, xhr, $form) {
                if (!result.error) {
                    $('#modal-send-email-report').modal('hide');
                    Swal.fire({
                        title: result.message,
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
                        title: result.message,
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

    let url = "/fin-rpt/submission/get-data/user-list?usergroupid=" + groupuser + "&userlevel=" + 0 + "&status=" + 0;
    dt_profile.clear().draw()
    dt_profile.ajax.url(url).load();
});

$('#modal-send-email-report').on('hidden.bs.modal', function () {
    $('#modal-send-email-report').val('')
});

$('#dt_profile').on( 'dblclick', 'tr', function () {
    let data = dt_profile.row(this).data();

    var idx = dt_send_email
        .columns(0)
        .data()
        .eq(0)
        .indexOf(data.id);

    if (idx === -1) {
        let dprofile = []
        dprofile["id"] = data.id;
        dprofile["email"] = data.email;
        dprofile["username"] = data.username;
        dprofile["usergroupname"] = data.usergroupname;
        dprofile["statusname"] = data.statusname;
        dt_send_email.row.add(dprofile).draw();
        dt_send_email.order([3, 'asc']).draw();
    }

    var dprofiles = []
    dprofiles.push({
        id: data.usergroupname,
        text: data.usergroupname
    })
    $('#filter_groupuser_grouping').select2({
        dropdownParent: $("#modal-send-email-report"),
        placeholder: "Select an User Group Menu",
        allowClear: true,
        data: dprofiles
    });
});

$('#dt_send_email').on( 'dblclick', 'tr', function () {
    $('#btn_del').html("Remove All");
    var tr = $(this).closest('tr');
    var groups = tr.find("#group").text();

    if (groups != "") {
        //console.log("Tidak Terjadi Apa apa")
    } else {
        let trindex = dt_send_email.row(tr).index();
        dt_send_email.row(trindex).remove().draw();
    }
});

const getListUserGroup = (usergroupid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/submission/list/usergroup",
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
    console.log('test')
    $('#btn_del').html("Remove All");

    dt_profile.rows().every(function () {

        let datas = dt_profile.row(this).data();

        var idx = dt_send_email
            .columns(0)
            .data()
            .eq(0)
            .indexOf(datas.id);

        var dprofiles = []
        dprofiles.push({
            id: datas.usergroupname,
            text: datas.usergroupname
        })
        $('#filter_groupuser_del').select2({
            dropdownParent: $("#modal-send-email-report"),
            placeholder: "Select an User Group Menu",
            allowClear: true,
            data: dprofiles
        });

        if (idx === -1 && datas.statusname != "Inactive") {
            dt_send_email.row.add(this.data()).draw();
            dataEmailSend.push(this.data());

        }
    });

    dt_send_email
        .order([3, 'asc'])
        .draw();
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
        $('#filter_groupuser_del').val(null).trigger('change');
        $("#filter_groupuser_grouping option[value='" + del_group + "']").remove();
        $('#btn_del').html("Remove All");
    }
});
