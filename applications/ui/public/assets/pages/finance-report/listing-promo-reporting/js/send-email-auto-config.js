'use strict';
var validator;
var configurationId;
var groupColumn = 2;
var dataEmailSend = [];
var defaultConfiguration = [];

var targetAttribute = document.querySelector(".dt_send_email_config");
var blockTableSendEmail = new KTBlockUI(targetAttribute, {
    message: '<div class="blockui-message"><span class="spinner-border text-optima"></span> Loading...</div>',
});

$(document).ready(function () {

    $.ajaxSetup({
        headers: {
            'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
        }
    });

    getListUserGroupConfig();
    $('#btn_remove_config').attr('disabled', true);
    $('#btn_add_all_config').attr('disabled', true);

    validator = FormValidation.formValidation(document.getElementById('form_promo_approval_config'), {
        fields: {
            dt1: {
                validators: {
                    notEmpty: {
                        message: "Enter Date Reminder 1"
                    },
                    greaterThan: {
                        min: 1,
                        message: "Please enter a value greater than or equal to 1."
                    },
                    lessThan: {
                        max: 31,
                        message: "Please enter a value less than or equal to 31.."
                    }
                }
            },
            dt2: {
                validators: {
                    notEmpty: {
                        message: "Enter Date Reminder 2"
                    },
                    greaterThan: {
                        min: 1,
                        message: "Please enter a value greater than or equal to 1."
                    },
                    lessThan: {
                        max: 31,
                        message: "Please enter a value less than or equal to 31.."
                    }
                }
            },
        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    })

    dt_profile_config = $('#dt_profile_config').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        processing: true,
        serverSide: false,
        searching: true,
        paging: false,
        scrollCollapse: true,
        ordering: false,
        scrollY: "35vh",
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

    dt_send_email_config = $('#dt_send_email_config').DataTable({
        dom:
            "<'row'<'dt_toolbar'>>" +
            "<'row'<'col-sm-12'Rtr>>",
        processing: true,
        serverSide: false,
        searching: true,
        paging: false,
        scrollCollapse: true,
        scrollY: "35vh",
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
                        '<tr  style="background-color:#94b5e8"><td id="group" colspan="4">' + group + '</td></tr>'
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

$('.numberonly').keypress(function (e) {
    let charCode = (e.which) ? e.which : event.keyCode
    if (String.fromCharCode(charCode).match(/[^0-9]/g))
        return false;

});

$('#dt_profile_config_search').on('keyup', function () {
    dt_profile_config.search(this.value).draw();
});

$('#dt_send_email_config_search').on('keyup', function () {
    dt_send_email_config.search(this.value).draw();
});

$('#autorun').on('change', function () {
    if($(this).prop('checked')) {
        $('#dt1').removeClass('form-control-solid-bg');
        $('#dt1').attr('readonly', false);

        $('#dt2').removeClass('form-control-solid-bg');
        $('#dt2').attr('readonly', false);

        $('#btn_remove_config').attr('disabled', false);
        $('#btn_add_all_config').attr('disabled', false);
        $('#end_of_month').attr('disabled', false);
    } else {
        $('#dt1').addClass('form-control-solid-bg');
        $('#dt1').attr('readonly', true);

        $('#dt2').addClass('form-control-solid-bg');
        $('#dt2').attr('readonly', true);

        $('#btn_remove_config').attr('disabled', true);
        $('#btn_add_all_config').attr('disabled', true);
        $('#end_of_month').prop('checked', false);
        $('#end_of_month').attr('disabled', true);

        dt_send_email_config.clear().draw();
        dt_send_email_config.rows.add(defaultConfiguration).draw();
    }
});

$('#end_of_month').on('change', function () {
    if($(this).prop('checked')) {
        $('#dt2').addClass('form-control-solid-bg');
        $('#dt2').attr('readonly', true);
    } else {
        $('#dt2').removeClass('form-control-solid-bg');
        $('#dt2').attr('readonly', false);
    }
});

$('#dt_send_email_config_submit').on('click', function() {
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let e = document.querySelector("#dt_send_email_config_submit");

            let rowdata = dt_send_email_config
                .rows()
                .data();

            let dataEmailSave = []
            let objEmail = {}
            $.each(rowdata, function (index, rowId) {
                objEmail = {};
                objEmail.email = rowId.email;
                objEmail.userName = rowId.userName;
                objEmail.userGroupName = rowId.userGroupName;
                objEmail.statusName = 'active';
                dataEmailSave.push(objEmail);
            });

            if (dataEmailSave.length == 0) {
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
                let emailSave = JSON.stringify(dataEmailSave);

                let formData = new FormData($('#form_promo_approval_config')[0]);
                formData.append('id', configurationId);
                formData.append('emailsave', emailSave);

                $.ajax({
                    url         : '/fin-rpt/listing-promo-reporting/configuration',
                    data        : formData,
                    type        : 'POST',
                    async       : true,
                    dataType    : 'JSON',
                    cache       : false,
                    contentType : false,
                    processData : false,
                    beforeSend: function() {
                        e.setAttribute("data-kt-indicator", "on");
                        e.disabled = !0;
                    },
                    success: function(result, status, xhr, $form) {
                        if (!result.error) {
                            $('#modal-send-email-auto-config').modal('hide');
                            Swal.fire({
                                title: result.message,
                                icon: "success",
                                confirmButtonText: "OK",
                                customClass: {confirmButton: "btn btn-optima"}
                            }).then(function (result) {
                                $('#dt_profile_config_search').val('');
                                $('#dt_send_email_config_search').val('');
                                dt_send_email_config.clear().draw();
                            });
                            defaultConfiguration = dataEmailSave;
                        } else {
                            $('#modal-send-email-auto-config').modal('hide');
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
        } else {
        let form = document.getElementById("form_promo_approval_config");
        let elements = form.elements;
        let txt = "";
        for (let i = 0, len = elements.length; i < len; ++i) {
            let el_class = elements[i];
            if (el_class.classList.contains('is-invalid')) {
                txt += el_class.nextElementSibling.innerText + "<br/>";
            }
        }
        Swal.fire({
            title: "Data not valid",
            html: txt,
            icon: "warning",
            confirmButtonText: "OK",
            allowOutsideClick: false,
            allowEscapeKey: false,
            customClass: {confirmButton: "btn btn-optima"}
            });
        }
    });
});

$('#filter_groupuser_config').on('change', async function () {
    let groupuser = $(this).val();
    if (groupuser == "" || groupuser == null) { groupuser = "all" };

    let url = "/fin-rpt/listing-promo-reporting/get-data/user-list?usergroupid=" + groupuser + "&userlevel=" + 0 + "&status=" + 0;
    dt_profile_config.clear().draw()
    dt_profile_config.ajax.url(url).load();
});

$('#filter_groupuser_grouping_config').on('change', async function () {
    dt_send_email_config.search(this.value).draw();
});

$('#modal-send-email-auto-config').on('hidden.bs.modal', function () {
    $('#modal-send-email-auto-config').val('')
});

$('#dt_profile_config').on( 'dblclick', 'tr', function () {
    let data = dt_profile_config.row(this).data();

    let idx = dt_send_email_config
        .columns(0)
        .data()
        .eq(0)
        .indexOf(data.email);

    if($('#autorun').prop('checked')){
        if (idx === -1 && data.email !== null && data.statusName === 'active' && data.userGroupName !== null) {
            let dprofile = []
            dprofile["email"] = data.email;
            dprofile["userName"] = data.userName;
            dprofile["userGroupName"] = data.userGroupName;
            dprofile["statusName"] = data.statusName;
            dt_send_email_config.row.add(dprofile).draw();
        }

        let dprofiles = []
        dprofiles.push({
            id: data.userGroupName,
            text: data.userGroupName
        })
        $('#filter_groupuser_grouping_config').select2({
            dropdownParent: $("#modal-send-email-auto-config"),
            placeholder: "Select an User Group Menu",
            allowClear: true,
            data: dprofiles
        });
        $('#filter_groupuser_grouping_config').val('').trigger('change');
    }
});

$('#dt_send_email_config').on( 'dblclick', 'tr', function () {
    $('#btn_del_config').html("Remove All");
    let tr = $(this).closest('tr');

    let trindex = dt_send_email_config.row(tr).index();
    dt_send_email_config.row(trindex).remove().draw();
});

const getDataConfiguration = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/fin-rpt/listing-promo-reporting/get-data/promo-approval-reminder",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                configurationId = result.data.id;
                if (result.data.autorun === true) {
                    $('#autorun').prop('checked', result.data.autorun);

                    $('#dt1').val(result.data.dt1);
                    $('#dt1').removeClass('form-control-solid-bg');
                    $('#dt1').attr('readonly', false);

                    $('#dt2').val(result.data.dt2);
                    $('#dt2').removeClass('form-control-solid-bg');
                    $('#dt2').attr('readonly', false);

                    $('#end_of_month').prop('checked', result.data.eod);
                    $('#end_of_month').attr('disabled', false);

                    $('#btn_remove_config').attr('disabled', false);
                    $('#btn_add_all_config').attr('disabled', false);
                } else {
                    $('#autorun').prop('checked', result.data.autorun);

                    $('#dt1').val(result.data.dt1);
                    $('#dt1').addClass('form-control-solid-bg');
                    $('#dt1').attr('readonly', true);

                    $('#dt2').val(result.data.dt2);
                    $('#dt2').addClass('form-control-solid-bg');
                    $('#dt2').attr('readonly', true);

                    $('#end_of_month').prop('checked', result.data.eod);
                    $('#end_of_month').attr('disabled', true);

                    $('#btn_remove_config').attr('disabled', true);
                    $('#btn_add_all_config').attr('disabled', true);
                }

                if(result.data.configEmail) {
                    dt_send_email_config.clear().draw();
                    dt_send_email_config.rows.add(result.data.configEmail).draw();
                }

                let dprofiles = [];
                for (let i = 0; i < result.data.configEmail.length; i++) {
                    defaultConfiguration.push(result.data.configEmail[i]);
                    dprofiles.push({
                        id: result.data.configEmail[i].userGroupName,
                        text: result.data.configEmail[i].userGroupName
                    });
                }

                $('#filter_groupuser_grouping_config').select2({
                    placeholder: "Select an User Group Menu",
                    width: '100%',
                    data: dprofiles
                });

                $('#filter_groupuser_grouping_config').val('').trigger('change');
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

const getListUserGroupConfig = (usergroupid) => {
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
                $('#filter_groupuser_config').select2({
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

$('#btn_add_all_config').on('click', function() {
    $('#btn_del_config').html("Remove All");

    dt_profile_config.rows().every(function () {

        let datas = dt_profile_config.row(this).data();

        let idx = dt_send_email_config
            .columns(0)
            .data()
            .eq(0)
            .indexOf(datas.email);

        if (idx === -1 && datas.email !== null && datas.statusName === 'active' && datas.userGroupName !== null) {
            dt_send_email_config.row.add(this.data()).draw();
            dataEmailSend.push(this.data());
            let dprofiles = []
            dprofiles.push({
                id: datas.userGroupName,
                text: datas.userGroupName
            })
            $('#filter_groupuser_grouping_config').select2({
                dropdownParent: $("#modal-send-email-auto-config"),
                placeholder: "Select an User Group Menu",
                allowClear: true,
                data: dprofiles
            });
            $('#filter_groupuser_grouping_config').val('').trigger('change');
        }
    });
});

$('#btn_remove_config').on('click', function() {
    var del_group = $('#filter_groupuser_grouping_config').val();
    if ($('#btn_del_config').html() === "Remove All") {
        dataEmailSend = []
        dt_send_email_config.rows().remove().draw();
        $('#filter_groupuser_grouping_config').empty();
    } else {
        dt_send_email_config.rows(function (idx, data, node) {
            return data.userGroupName === del_group
        }).remove().draw();
        $('#filter_groupuser_grouping_config').val(null).trigger('change');
        $("#filter_groupuser_grouping_config option[value='" + del_group + "']").remove();
        $('#btn_del_config').html("Remove All");
    }
});
