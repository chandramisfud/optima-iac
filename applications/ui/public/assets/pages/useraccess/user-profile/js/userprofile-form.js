'use strict';

let validator, method, profileid, usergroupid, userlevel;
let swalTitle = "User Profile";

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
    method = url_str.searchParams.get("method");
    profileid = url_str.searchParams.get("profileid");

    if (method === 'update') {
        disableButtonSave();
        blockUI.block();
        Promise.all([getGroupMenu(), getDistributor(), getCategory(), getChannel()]).then(async () => {
            await getData(profileid);
            await getGroupRights(usergroupid);
            $('#usergrouplevel').val(userlevel).trigger('change');
            $('#profileid').attr('readonly', true);
            enableButtonSave();
            blockUI.release();
        });
    } else {
        blockUI.block();
        disableButtonSave();
        Promise.all([getGroupMenu(), getDistributor(), getCategory(), getChannel()]).then(() => {
            enableButtonSave();
            blockUI.release();
        });
    }
    const form = document.getElementById('form_userprofile');

    validator = FormValidation.formValidation(form, {
        fields: {
            profileid: {
                validators: {
                    notEmpty: {
                        message: "The Profile ID is required"
                    },
                    stringLength: {
                        max: 50,
                        message: 'Profile ID must be less than 50 characters',
                    }
                }
            },
            profilename: {
                validators: {
                    notEmpty: {
                        message: "Profile Name is required"
                    },
                    stringLength: {
                        max: 50,
                        message: 'Profile Name must be less than 50 characters',
                    }
                }
            },
            email: {
                validators: {
                    notEmpty: {
                        message: "Email is required"
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_.+-]+@(?:(?:[a-zA-Z0-9-]+\.)?[a-zA-Z]+\.)?(danone.com|tigaraksa.co.id|thetempogroup.com|aladdincommerce.co.id|acommerce.asia|orami.com|aplcare.com|samb.co.id|zuelligpharma.com|xvautomation.com)$/i,
                        message: "Sorry, I've enabled very strict email validation"
                    }
                }
            },
            department: {
                validators: {
                    notEmpty: {
                        message: "Department is required"
                    },
                    stringLength: {
                        max: 50,
                        message: 'Department must be less than 50 characters',
                    }
                }
            },
            jobtitle: {
                validators: {
                    notEmpty: {
                        message: "Job Title is required"
                    },
                    stringLength: {
                        max: 255,
                        message: 'Job Title must be less than 255 characters',
                    }
                }
            },
            usergroupid: {
                validators: {
                    notEmpty: {
                        message: "User Group Menu is required"
                    },
                }
            },
            usergrouplevel: {
                validators: {
                    notEmpty: {
                        message: "User Group Rights is required"
                    },
                }
            },
            distributor: {
                validators: {
                    notEmpty: {
                        message: "Coverage is required"
                    },
                }
            },
            category: {
                validators: {
                    notEmpty: {
                        message: "Category is required"
                    },
                }
            },

        },
        plugins: {
            trigger: new FormValidation.plugins.Trigger,
            bootstrap: new FormValidation.plugins.Bootstrap5({rowSelector: ".fv-row"})
        }
    });
});

$('#btn_back').on('click', function() {
    window.location.href = '/useraccess/profile';
});

$('#usergroupid').on('change', async function(e) {
    validator.revalidateField('usergroupid');
    let userGroupId = $(this).val();
    let elUserGroupLevel = $('#usergrouplevel');
    elUserGroupLevel.empty();
    if ($(this).val() !== "") await getGroupRights(userGroupId);
    elUserGroupLevel.val('').trigger('change');
});

$('#usergrouplevel').on('change', async function(e) {
    validator.revalidateField('usergrouplevel');
});

$('#distributor').on('change', async function(e) {
    validator.revalidateField('distributor');
    let arrDistributor = $(this).val();
    if (arrDistributor[0] === "") {
        $(this).val([]).trigger('change');
    }
});

$('#category').on('change', async function(e) {
    validator.revalidateField('category');
    let arrCategory = $(this).val();
    if (arrCategory[0] === "") {
        $(this).val([]).trigger('change');
    }
});

$('#btn_save').on('click', function() {
    let e = document.querySelector("#btn_save");
    validator.validate().then(function (status) {
        if (status === "Valid") {
            let formData = new FormData($('#form_userprofile')[0]);
            let url = '/useraccess/profile/save';
            if (method === "update") {
                url = '/useraccess/profile/update';
                formData.append('id', profileid);
            }

            let distributor = $("#distributor").select2().val();

            let listDistributor = [];
            for (let i = 0; i < distributor.length; i++) {
                listDistributor.push({
                    userId : $('#profileid').val(),
                    distributorId : distributor[i]
                });
            }

            let category = $("#category").select2().val();
            let listCategory = [];
            for (let i = 0; i < category.length; i++) {
                listCategory.push(category[i]);
            }

            let channel = $("#channelId").select2().val();
            let listChannel = [];
            for (let i = 0; i < channel.length; i++) {
                listChannel.push(channel[i]);
            }

            formData.set('distributorid', JSON.stringify(listDistributor));
            formData.set('categoryId', JSON.stringify(listCategory));
            formData.set('channelId', JSON.stringify(listChannel));

            $.get('/refresh-csrf').done(function(data) {
                $('meta[name="csrf-token"]').attr('content', data)
                $.ajaxSetup({
                    headers: {
                        'X-CSRF-TOKEN': $('meta[name="csrf-token"]').attr('content')
                    }
                });
                $.ajax({
                    url: url,
                    data: formData,
                    type: 'POST',
                    async: true,
                    dataType: 'JSON',
                    cache: false,
                    contentType: false,
                    processData: false,
                    beforeSend: function () {
                        e.setAttribute("data-kt-indicator", "on");
                        e.disabled = !0;
                    },
                    success: function (result, status, xhr, $form) {
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
                                window.location.href = '/useraccess/profile';
                            });
                        } else {
                            Swal.fire({
                                title: swalTitle,
                                text: result.message,
                                icon: "warning",
                                confirmButtonText: "OK",
                                allowOutsideClick: false,
                                allowEscapeKey: false,
                                customClass: {confirmButton: "btn btn-optima"}
                            });
                        }
                    },
                    complete: function () {
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

const getData = (profileid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/useraccess/profile/get-data/id",
            type: "GET",
            data: {profileId:profileid},
            dataType: "JSON",
            success: function (result) {
                if (!result.error) {
                    let values = result.data;
                    $('#txt_info_method').text('Edit Data ' + values.id);
                    usergroupid = values.usergroupid;
                    userlevel = values.userlevel;
                    $('#profileid').val(values.id);
                    $('#profilename').val(values.username);
                    $('#email').val(values.email);
                    $('#department').val(values.department);
                    $('#jobtitle').val(values.jobtitle);
                    $('#usergroupid').val(usergroupid).trigger('change');
                    if (values.distributorlist) {
                        let distributor_list = [];
                        for (let i=0; i < values.distributorlist.length; i++) {
                            distributor_list.push(values.distributorlist[i].distributorId);
                        }
                        $('#distributor').val(distributor_list).trigger('change');
                    }
                    if (values.categoryList) {
                        let category_list = [];
                        for (let i=0; i < values.categoryList.length; i++) {
                            category_list.push(values.categoryList[i].categoryId);
                        }
                        $('#category').val(category_list).trigger('change');
                    }
                    if (values.channelList) {
                        let channel_list = [];
                        for (let i=0; i < values.channelList.length; i++) {
                            channel_list.push(values.channelList[i].channelId);
                        }
                        $('#channelId').val(channel_list).trigger('change');
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

const getGroupMenu = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url         : "/useraccess/profile/usergroupmenu",
            type        : "GET",
            dataType    : 'json',
            async       : true,
            success: function(result) {
                var data = [];
                for (let j = 0, len = result.data.length; j < len; ++j){
                    data.push({
                        id: result.data[j].usergroupid,
                        text: result.data[j].usergroupname
                    });
                }
                $('#usergroupid').select2({
                    placeholder: "Select a User Group Menu",
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

const getGroupRights = (usergroupid) => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/useraccess/profile/usergrouprights/usergroupid",
            type: "GET",
            dataType: 'json',
            data: {usergroupid: usergroupid},
            async: true,
            success: function (result) {
                let comp_usergrouplevel = $('#usergrouplevel');
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j) {
                    data.push({
                        id: result.data[j].userlevelid,
                        text: result.data[j].userlevelname
                    });
                }
                comp_usergrouplevel.select2({
                    placeholder: "Select a Group Rights",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
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

const getDistributor = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/useraccess/profile/distributor",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j) {
                    data.push({
                        id: result.data[j].id,
                        text: result.data[j].longDesc
                    });
                }
                $('#distributor').select2({
                    placeholder: "Select a Distributor",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
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

const getCategory = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/useraccess/profile/category",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j) {
                    data.push({
                        id: result.data[j].categoryId,
                        text: result.data[j].categoryShortDesc + " - " + result.data[j].categoryLongDesc
                    });
                }
                $('#category').select2({
                    placeholder: "Select a Category",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
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

const getChannel = () => {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: "/useraccess/profile/channel",
            type: "GET",
            dataType: 'json',
            async: true,
            success: function (result) {
                let data = [];
                for (let j = 0, len = result.data.length; j < len; ++j) {
                    data.push({
                        id: result.data[j]['id'],
                        text: result.data[j]['longDesc']
                    });
                }
                $('#channelId').select2({
                    placeholder: "Select a Channel",
                    width: '100%',
                    data: data
                });
            },
            complete: function() {
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
