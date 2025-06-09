$(document).ready(function () {
    defaultConfig(this);
    pageSetUp();
});

function defaultConfig(parent) {
    parent?.querySelectorAll('.datepicker').forEach(function (e) {
        $(e).datepicker({
            dateFormat: 'dd/mm/yy',
            autoclose: true
        });
    })
    parent?.querySelectorAll('.select2').forEach(function (e) {
        $(e).select2({
            width: "100%",
            theme: 'bootstrap-5'
        });
    })
    parent?.querySelectorAll('.popup').forEach(function (e) {
        $(e).on("click", function (event) {
            event.preventDefault();
            modelPopup(this);
        })
    })
    parent?.querySelectorAll('.select-value').forEach(function (e) {
        $(e).on("change", function (event) {
            event.preventDefault();
            this.form.submit();
        })
    })
}

function ControlDisabled(fromId) {
    $('#' + fromId).find(':button[type=submit]').prop('disabled', true);
    $('#' + fromId).find(':button[type=button]').prop('disabled', true);
}
function ControlEnabled(fromId) {
    $('#' + fromId).find(':button[type=submit]').prop('disabled', false);
    $('#' + fromId).find(':button[type=button]').prop('disabled', false);
}
function postData(formId, postUrl, callBack) {
    $('#' + formId).off('submit').on('submit', function (e) {
        ControlDisabled(formId);
        if ($("#" + formId).valid()) {
            e.preventDefault();
            let myForm = document.getElementById(formId);
            let formData = new FormData(myForm);
            $('.wait').addClass("loading");
            $.ajax({
                url: postUrl,
                type: "POST",
                data: formData,
                contentType: false,
                processData: false,
                success: function (data) {
                    hideLoading();
                    if (typeof (data.isSuccessed) === 'boolean') {
                        ShowMessageData(data);
                        callBack(data);
                        dropModal();
                    }
                    else {
                        ControlEnabled(formId);
                        callBack(data);
                    }
                },
                error: function (jqXHR) {
                    hideLoading();
                    ControlEnabled(formId);
                },
            });
        }
        else {
            ControlEnabled(formId);
        }
    });
}

function postValue(postUrl, dataInput, callBack) {
    $('.wait').addClass("loading");
    $.ajax({
        url: postUrl,
        type: "POST",
        data: dataInput,
        success: function (data) {
            hideLoading();
            if (typeof (data.isSuccessed) === 'boolean') {
                ShowMessageData(data);
                callBack(data)
            }
            else {
                callBack(data);
            }
        },
        error: function (jqXHR) {
            hideLoading();
        },
    });
}
function loadData(url) {
    dropModal();
    showLoading();
    if (url != "") {
        $.ajax({
            url: url,
            type: "GET",
            success: function (data) {
                if (data != null) {
                    $('.content').html(data);
                }
                $(".popup").on('click', function (e) {
                    modelPopup(this);
                });
                hideLoading();
                $('.select2').select2();
                $(".callout").fadeOut(2500);
            }
        });
    }
}
function loadContent(url, element, callback = null) {
    showLoading();
    if (!isNullOrWhiteSpace(url)) {
        $.ajax({
            url: decodeURIComponent(url),
            type: "GET",
            success: function (data) {
                hideLoading();
                if (!isNullOrWhiteSpace(data)) {

                    $(element).html(data);
                    defaultConfig(element);
                }
                if (callback) {
                    callback();
                }
            },
            error: function () {
                hideLoading();
                if (callback) {
                    callback();
                }
            }
        });
    }
}

function loadHtml(url, params, element, callBack = null) {
    showLoading();
    if (url != "") {
        $.ajax({
            url: url,
            data: params,
            type: "GET",
            success: function (data) {
                hideLoading();
                $(element).html(data);
                defaultConfig(element)
                if (callBack) {
                    callBack(data);
                }
            },
            error: function (jqXHR) {
                hideLoading();
            },
        });
    }
}
function loadValue(url, callBack) {
    $.ajax({
        url: url,
        type: "GET",
        success: function (data) {
            callBack(data);
        }, error: function (jqXHR) {
            ShowMessage(jqXHR.responseText);
            hideLoading();
        },
    });
}
function loadOption(url, showId) {
    $.ajax({
        url: url,
        type: "GET",
        success: function (data) {
            $('#' + showId).html(data);
        }
    });
}

function updateData(postUrl, id, value, callBack, elem) {
    $.ajax({
        url: postUrl,
        type: "POST",
        data: { "id": id, "value": value },
        success: function (data) {
            ShowMessageData(data);
            if (!data.isSuccessed) {
                if (callBack) {
                    callBack(elem, data)
                }
            }
            
        }
    });
}
function DeleteData(postUrl, id, callBack) {
    $.ajax({
        url: postUrl,
        type: "POST",
        data: { "id": id },
        success: function (data) {
            ShowMessageData(data);
            callBack(data);
        },
        error: function (jqXHR) {
            ShowMessageData(jqXHR.responseText);
        },
    });

}
function submitData(postUrl, data, callback = null) {
    showLoading();
    $.ajax({
        url: postUrl,
        type: "POST",
        data: data,
        success: function (data) {
            hideLoading();
            ShowMessageData(data);
            if (callback) {
                callback();
            }
        }
    });
}
function loadMessage(data) {
    $(".message").html(data);
    dropModal();
}
function getUrlVars(url) {
    var myJson = {};
    if (url != '') {
        var hash;
        myJson['resultUrl'] = url;

        var hashes = url.slice(url.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            myJson[hash[0]] = hash[1] != null ? hash[1].replace("+", " ") : "";
        }
    }
    return myJson;
}
function showLoading() {
    $('.wait').addClass("loading");
}
function hideLoading() {
    $('.wait').removeClass("loading");
}

function showProgressBar(value = 0) {
    const container = document.querySelector('.progress-container');
    if (!container.classList.contains('show-progress')) {
        container.classList.add('show-progress');
    }
    const bar = container.querySelector('.progress-bar');
    bar.setAttribute('aria-valuenow', value);
    bar.style.width = `${value}%`;
}
function hideProgressBar() {
    const container = document.querySelector('.progress-container');
    container.classList.remove('show-progress');
    const bar = container.querySelector('.progress-bar');
    bar.setAttribute('aria-valuenow', 0);
    bar.style.width = 0;
}
function modelPopup(reff) {
    var url = $(reff).data('url');
    var input = $(reff).data('link');
    var type = $(reff).data('type');

    var myModal = new bootstrap.Modal(document.getElementById("modal"), {});

    var data = getUrlVars(decodeURIComponent(input));

    $('#modal .modal-dialog').removeClass('modal-sm');
    $('#modal .modal-dialog').removeClass('modal-lg');
    $('#modal .modal-dialog').removeClass('modal-xl');
    $('#modal .modal-dialog').html('');
    $.ajax({
        url: url,
        type: "GET",
        data: data,
        success: function (data) {
            if (typeof (data.isSuccessed) === 'boolean') {
                dropModal();
                ShowMessageData(data);
            }
            else {
                if (data != null) {
                    $('#modal .modal-dialog').html(data);
                    /*$('#modal').modal("show");*/
                    myModal.show();
                    if (type)
                        $('#modal .modal-dialog').addClass(type)
                }
            }
            hideLoading();
        },
        error: function (jqXHR) {
            $(".modal-backdrop").remove();
            myModal.hide();
            $('#modal .modal-dialog').html('');
            dropModal();
            hideLoading();
        },
    });
}

function dropModal() {
    $('#modal').modal('hide');
    $('#modal .modal-dialog').removeClass('modal-lg', 'modal-sm');
    $('#modal .modal-dialog').html('');
}

function ConvertDMSToDD(degrees, minutes, seconds, direction) {
    var dd = Number(degrees) + Number(minutes) / 60 + Number(seconds) / (60 * 60);

    if (direction == "S" || direction == "W") {
        dd = dd * -1;
    } // Don't do anything for N or E
    return dd;
}
function ParseDMS(input) {
    var parts = input.split(/[^\d\w\.]+/);
    return ConvertDMSToDD(parts[0], parts[1], parts[2], parts[3]);
}
var file_type = "doc,docx,xls,xlsx,pdf,jpg";
function CheckFileTypeUpload(idfile, id_namefile) {

    file = $("#" + idfile).val();
    filetype_upload = file.split(".");
    typefile = filetype_upload[filetype_upload.length - 1];
    var filetype_check = file_type.split(",");
    check = 0;
    for (i = 0; i < filetype_check.length; i++) {
        if (typefile == filetype_check[i]) { check = check + 1; break; }
    }
    if (check == 0) {
        $("span[data-valmsg-for='" + idfile + "']").html("Định dạng file đính kèm không hợp lệ! Vui lòng chỉ chọn các định dạng: " + file_type);
        $("#" + idfile).val("");
        $("#" + id_namefile).val("");

    } else {
        $("#" + id_namefile).val(file);
    }
}
var file_type_imp = "xls,xlsx";
function IPPhuLucQuyetDinh(idfile, id_namefile) {

    file = $("#" + idfile).val();
    filetype_upload = file.split(".");
    typefile = filetype_upload[filetype_upload.length - 1];
    var filetype_check = file_type_imp.split(",");
    check = 0;
    for (i = 0; i < filetype_check.length; i++) {
        if (typefile == filetype_check[i]) { check = check + 1; break; }
    }
    if (check == 0) {
        $("span[data-valmsg-for='" + idfile + "']").html("Định dạng file đính kèm không hợp lệ! Vui lòng chỉ chọn các định dạng: " + file_type_imp);
        $("#" + idfile).val("");
        $("#" + id_namefile).val("");

    } else {
        $("#" + id_namefile).val(file);
    }
}
function autocomplete(inp, arr, Callback) {
    /*the autocomplete function takes two arguments,
    the text field element and an array of possible autocompleted values:*/
    var currentFocus;
    /*execute a function when someone writes in the text field:*/
    inp.addEventListener("input", function (e) {
        var a, b, i, val = this.value;
        /*close any already open lists of autocompleted values*/
        closeAllLists();
        if (!val) { return false; }
        currentFocus = -1;
        /*create a DIV element that will contain the items (values):*/
        a = document.createElement("DIV");
        a.setAttribute("id", this.id + "autocomplete-list");
        a.setAttribute("class", "autocomplete-items");
        /*append the DIV element as a child of the autocomplete container:*/
        this.parentNode.appendChild(a);
        /*for each item in the array...*/
        for (i = 0; i < arr.length; i++) {
            /*check if the item starts with the same letters as the text field value:*/
            if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
                /*create a DIV element for each matching element:*/
                b = document.createElement("DIV");
                /*make the matching letters bold:*/
                b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
                b.innerHTML += arr[i].substr(val.length);
                /*insert a input field that will hold the current array item's value:*/
                b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
                /*execute a function when someone clicks on the item value (DIV element):*/
                b.addEventListener("click", function (e) {
                    /*insert the value for the autocomplete text field:*/
                    inp.value = this.getElementsByTagName("input")[0].value;
                    /*close the list of autocompleted values,
                    (or any other open lists of autocompleted values:*/
                    closeAllLists();
                });
                a.appendChild(b);
            }

        }
    });
    /*execute a function presses a key on the keyboard:*/
    inp.addEventListener("click", function (e) {
        var a, b, i, val = this.value;
        closeAllLists();
        currentFocus = -1;
        /*close any already open lists of autocompleted values*/
        a = document.createElement("DIV");
        a.setAttribute("id", this.id + "autocomplete-list");
        a.setAttribute("class", "autocomplete-items");
        this.parentNode.appendChild(a);
        for (i = 0; i < arr.length; i++) {
            /*check if the item starts with the same letters as the text field value:*/
            b = document.createElement("DIV");
            /*make the matching letters bold:*/
            b.innerHTML = "<strong>" + arr[i] + "</strong>";
            /*insert a input field that will hold the current array item's value:*/
            b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
            /*execute a function when someone clicks on the item value (DIV element):*/
            b.addEventListener("click", function (e) {

                var val = this.getElementsByTagName("input")[0].value;
                /*insert the value for the autocomplete text field:*/
                inp.value = val;
                /*close the list of autocompleted values,
                (or any other open lists of autocompleted values:*/
                closeAllLists();
                Callback(val)
            });
            a.appendChild(b);
        }
    });

    inp.addEventListener("click", function (e) {

        var x = document.getElementById(this.id + "autocomplete-list");
        if (x) x = x.getElementsByTagName("div");
        if (e.keyCode == 40) {
            /*If the arrow DOWN key is pressed,
            increase the currentFocus variable:*/
            currentFocus++;
            /*and and make the current item more visible:*/
            addActive(x);
        } else if (e.keyCode == 38) { //up
            /*If the arrow UP key is pressed,
            decrease the currentFocus variable:*/
            currentFocus--;
            /*and and make the current item more visible:*/
            addActive(x);
        } else if (e.keyCode == 13) {
            /*If the ENTER key is pressed, prevent the form from being submitted,*/
            e.preventDefault();
            if (currentFocus > -1) {
                /*and simulate a click on the "active" item:*/
                if (x) x[currentFocus].click();
            }
        }
    });
    function addActive(x) {

        /*a function to classify an item as "active":*/
        if (!x) return false;
        /*start by removing the "active" class on all items:*/
        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);
        /*add class "autocomplete-active":*/
        x[currentFocus].classList.add("autocomplete-active");
    }
    function removeActive(x) {
        /*a function to remove the "active" class from all autocomplete items:*/
        for (var i = 0; i < x.length; i++) {
            x[i].classList.remove("autocomplete-active");
        }
    }
    function closeAllLists(elmnt) {
        /*close all autocomplete lists in the document,
        except the one passed as an argument:*/
        var x = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < x.length; i++) {
            if (elmnt != x[i] && elmnt != inp) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
    }
    /*execute a function when someone clicks in the document:*/
}
function closeItems() {
    /*close all autocomplete lists in the document,
    except the one passed as an argument:*/
    var x = document.getElementsByClassName("autocomplete-items");
    for (var i = 0; i < x.length; i++) {
        x[i].parentNode.removeChild(x[i]);
    }
}


function ConvertMoney(value, returns) {    // chuyển số 00000000 sang định dạng tiền 00.000.000
    value = Numbers(value);
    str_rev = strrev(value);
    strplit = str_split(str_rev, 3);
    count = strplit.length;
    var res = '';
    for (i = 0; i < count; i++) {
        res += strplit[i] + ',';
    }
    res = res.substring(0, res.length - 1);
    res = strrev(res);
    $("#" + returns).val(res);
    return res
};
function ConvertMoneyByThis(value, id) {    // chuyển số 00000000 sang định dạng tiền 00.000.000
    value = Numbers(value);
    str_rev = strrev(value);
    strplit = str_split(str_rev, 3);
    count = strplit.length;
    var res = '';
    for (i = 0; i < count; i++) {
        res += strplit[i] + ',';
    }
    res = res.substring(0, res.length - 1);
    res = strrev(res);
    $(id).val(res);
    return res
};
function str_replace(string, search, replace) { //lặp ký tự
    return string.split(search).join(replace);
};
function str_split(string, split_length) {
    if (split_length === null) {
        split_length = 1;
    }
    if (string === null || split_length < 1) {
        return false;
    }
    string += '';
    var chunks = [],
        pos = 0,
        len = string.length;
    while (pos < len) {
        chunks.push(string.slice(pos, pos += split_length));
    }
    return chunks;
};
function strrev(string) {
    string = string + '';
    var grapheme_extend = /(.)([\uDC00-\uDFFF\u0300-\u036F\u0483-\u0489\u0591-\u05BD\u05BF\u05C1\u05C2\u05C4\u05C5\u05C7\u0610-\u061A\u064B-\u065E\u0670\u06D6-\u06DC\u06DE-\u06E4\u06E7\u06E8\u06EA-\u06ED\u0711\u0730-\u074A\u07A6-\u07B0\u07EB-\u07F3\u0901-\u0903\u093C\u093E-\u094D\u0951-\u0954\u0962\u0963\u0981-\u0983\u09BC\u09BE-\u09C4\u09C7\u09C8\u09CB-\u09CD\u09D7\u09E2\u09E3\u0A01-\u0A03\u0A3C\u0A3E-\u0A42\u0A47\u0A48\u0A4B-\u0A4D\u0A51\u0A70\u0A71\u0A75\u0A81-\u0A83\u0ABC\u0ABE-\u0AC5\u0AC7-\u0AC9\u0ACB-\u0ACD\u0AE2\u0AE3\u0B01-\u0B03\u0B3C\u0B3E-\u0B44\u0B47\u0B48\u0B4B-\u0B4D\u0B56\u0B57\u0B62\u0B63\u0B82\u0BBE-\u0BC2\u0BC6-\u0BC8\u0BCA-\u0BCD\u0BD7\u0C01-\u0C03\u0C3E-\u0C44\u0C46-\u0C48\u0C4A-\u0C4D\u0C55\u0C56\u0C62\u0C63\u0C82\u0C83\u0CBC\u0CBE-\u0CC4\u0CC6-\u0CC8\u0CCA-\u0CCD\u0CD5\u0CD6\u0CE2\u0CE3\u0D02\u0D03\u0D3E-\u0D44\u0D46-\u0D48\u0D4A-\u0D4D\u0D57\u0D62\u0D63\u0D82\u0D83\u0DCA\u0DCF-\u0DD4\u0DD6\u0DD8-\u0DDF\u0DF2\u0DF3\u0E31\u0E34-\u0E3A\u0E47-\u0E4E\u0EB1\u0EB4-\u0EB9\u0EBB\u0EBC\u0EC8-\u0ECD\u0F18\u0F19\u0F35\u0F37\u0F39\u0F3E\u0F3F\u0F71-\u0F84\u0F86\u0F87\u0F90-\u0F97\u0F99-\u0FBC\u0FC6\u102B-\u103E\u1056-\u1059\u105E-\u1060\u1062-\u1064\u1067-\u106D\u1071-\u1074\u1082-\u108D\u108F\u135F\u1712-\u1714\u1732-\u1734\u1752\u1753\u1772\u1773\u17B6-\u17D3\u17DD\u180B-\u180D\u18A9\u1920-\u192B\u1930-\u193B\u19B0-\u19C0\u19C8\u19C9\u1A17-\u1A1B\u1B00-\u1B04\u1B34-\u1B44\u1B6B-\u1B73\u1B80-\u1B82\u1BA1-\u1BAA\u1C24-\u1C37\u1DC0-\u1DE6\u1DFE\u1DFF\u20D0-\u20F0\u2DE0-\u2DFF\u302A-\u302F\u3099\u309A\uA66F-\uA672\uA67C\uA67D\uA802\uA806\uA80B\uA823-\uA827\uA880\uA881\uA8B4-\uA8C4\uA926-\uA92D\uA947-\uA953\uAA29-\uAA36\uAA43\uAA4C\uAA4D\uFB1E\uFE00-\uFE0F\uFE20-\uFE26]+)/g;
    string = string.replace(grapheme_extend, '$2$1'); // Temporarily reverse
    return string.split('').reverse().join('');
};
function Numbers(str) {
    return str.replace(/[^0-9]/gi, "");
}


String.prototype.replaceAll = function (find, replace) {
    var str = this;
    return str.replace(new RegExp(find.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&'), 'g'), replace);
};

/**
  * Checks the string if undefined, null, not typeof string, empty or space(s)
  * @param {any} str string to be evaluated
  * @returns {boolean} the evaluated result
*/
function isNullOrWhiteSpace(str) {
    return str === undefined
        || str === null
        || typeof str !== 'string'
        || str.match(/^\s*$/) !== null;
}
function convertViToEn(str) {
    str = str.toLowerCase();
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    // Some system encode vietnamese combining accent as individual utf-8 characters
    str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // Huyền sắc hỏi ngã nặng
    str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // Â, Ê, Ă, Ơ, Ư

    return str;
}

function convertToReplaceRoute(str) {
    var arr = str.split(' ');
    var title = "";
    if (arr.length > 40) {
        for (var i = 0; i < 40; i++) {
            title += arr[i] + " ";
        }
    }
    else {
        title = str;
    }
    let route = convertViToEn(title.trim());
    route = route.replace(/\W+/g, '-');
    return route;
}

/**
  * Mở trình quản lý file để chọn ảnh
  * @param {string} elementSelectId Id của hình ảnh hiển thị/Tên hiển thị
  * @param {string} inputId Id của input
  * @param {string} folderType Kiểu thư mục theo enum
  * @param {boolean} isOnlyImage Kiểu thư mục theo enum
  * @param {URLSearchParams} queryParams Query parameters cần thiết của url
*/
function openFileManager(elementSelectId, inputId, folderType, isChoseAndSave = false, isOnlyImage = true, queryParams = new URLSearchParams()) {
    const url = new URL('/Admin/Browse/', location.origin);
    url.searchParams.set('elementSelectId', elementSelectId)
    url.searchParams.set('inputId', inputId);
    url.searchParams.set('folderType', folderType);
    url.searchParams.set('isChoseAndSave', isChoseAndSave);
    if (!isOnlyImage) {
        url.searchParams.set('isOnlyImage', false);
    }

    if (queryParams.size !== 0) {
        queryParams.forEach(function (value, key) {
            url.searchParams.set(key, value);
        })
    }
    window.open(url.href, isOnlyImage ? "ImageBrowse" : "FileBrowse", "width=1330,height=700");
}

function openBrowseFile(elementSelectId, inputId, folderType, fileType = 0, isChoseAndSave = false, queryParams = new URLSearchParams()) {
    const url = new URL('/Admin/Browse/', location.origin);
    url.searchParams.set('elementSelectId', elementSelectId)
    url.searchParams.set('inputId', inputId);
    url.searchParams.set('folderType', folderType);
    url.searchParams.set('fileType', fileType);
    url.searchParams.set('isChoseAndSave', isChoseAndSave);
    if (queryParams.size !== 0) {
        queryParams.forEach(function (value, key) {
            url.searchParams.set(key, value);
        })
    }
    window.open(url.href, fileType == 1 ? "ImageBrowse" : "FileBrowse", "width=1330,height=700");
}
/**
  * Tạo file upload element khi thêm file đính kèm
  * @param {number} index Số thứ tự của file đính kèm
  * @param {string} inputName Giá trị name attribute của input file
*/
function createFileUploadContentElement(index) {
    const fileContainer = document.createElement('div');
    fileContainer.classList.add('mb-3', 'file');

    const group = document.createElement('div');
    group.classList.add('input-group');

    const fileTitle = document.createElement('input');
    fileTitle.id = `titlefile-${index}`;
    fileTitle.classList.add('form-control');
    fileTitle.setAttribute('style', 'width:70%;');
    fileTitle.setAttribute('placeholder', 'Tiêu đề');
    fileTitle.setAttribute('name', `Files[${index}].FileTitle`);

    const fileName = document.createElement('span');
    fileName.id = `file-name-${index}`;
    fileName.textContent = 'Chưa chọn';
    fileName.classList.add('form-control');
    fileName.setAttribute('style', 'width:30%;');

    const groupBtn = document.createElement('div');
    groupBtn.classList.add('input-group-btn');

    const file = document.createElement('input');
    file.id = `file-${index}`;
    file.setAttribute('name', `Files[${index}].FileId`);
    file.setAttribute('type', 'hidden');

    const fileDelete = document.createElement('input');
    fileDelete.id = `delete-file-${index}`;
    fileDelete.setAttribute('name', `Files[${index}].IsDelete`);
    fileDelete.setAttribute('type', 'hidden');
    fileDelete.setAttribute('value', 'false');

    const btnUpload = document.createElement('button');
    btnUpload.setAttribute('data-index', index);
    btnUpload.setAttribute('type', 'button');
    btnUpload.classList.add('btn', 'btn-success', 'btn-flat', 'choose-file');

    const iconUpload = document.createElement('i');
    iconUpload.classList.add('fa', 'fa-upload');
    btnUpload.appendChild(iconUpload);

    const btnDelete = document.createElement('button');
    btnDelete.setAttribute('type', 'button');
    btnDelete.classList.add('btn', 'btn-danger', 'btn-flat', 'delete-file');
    btnDelete.setAttribute('data-value', index);

    const iconDelete = document.createElement('i');
    iconDelete.classList.add('fa', 'fa-trash');
    btnDelete.appendChild(iconDelete);

    groupBtn.appendChild(file);
    groupBtn.appendChild(btnUpload);
    groupBtn.appendChild(btnDelete);

    group.appendChild(fileTitle);
    group.appendChild(fileName);
    group.appendChild(fileDelete);
    group.appendChild(groupBtn);

    fileContainer.appendChild(group);

    return fileContainer;
}
function createFileUploadElement(index, inputName) {
    const fileContainer = document.createElement('div');
    fileContainer.classList.add('mb-3', 'file');

    const group = document.createElement('div');
    group.classList.add('input-group');

    const fileName = document.createElement('span');
    fileName.id = `file-name-${index}`;
    fileName.classList.add('form-control');

    const groupBtn = document.createElement('div');
    groupBtn.classList.add('input-group-btn');

    const file = document.createElement('input');
    file.id = `file-${index}`;
    file.setAttribute('name', inputName);
    file.setAttribute('type', 'hidden');

    const btnUpload = document.createElement('button');
    btnUpload.setAttribute('data-index', index);
    btnUpload.setAttribute('type', 'button');
    btnUpload.classList.add('btn', 'btn-success', 'btn-flat', 'choose-file');

    const iconUpload = document.createElement('i');
    iconUpload.classList.add('fa', 'fa-upload');
    btnUpload.appendChild(iconUpload);

    const btnDelete = document.createElement('button');
    btnDelete.setAttribute('type', 'button');
    btnDelete.classList.add('btn', 'btn-danger', 'btn-flat', 'delete-file');

    const iconDelete = document.createElement('i');
    iconDelete.classList.add('fa', 'fa-trash');
    btnDelete.appendChild(iconDelete);

    groupBtn.appendChild(file);
    groupBtn.appendChild(btnUpload);
    groupBtn.appendChild(btnDelete);

    group.appendChild(fileName);
    group.appendChild(groupBtn);

    fileContainer.appendChild(group);

    return fileContainer;
}
function CoppyToClipboard(text) {

    if (navigator.clipboard != undefined) {//Chrome
        navigator.clipboard.writeText(text).then(function () {
            // showNotificationSuucess("Đã coppy!");
        }, function (err) {
            //showNotificationError("Coppy không thành công!");
        });
    }
    else if (window.clipboardData) { // Internet Explorer
        window.clipboardData.setData("Text", text);
    }


}
function ShowMessageData(data) {
    $.smallBox({
        title: "Thông báo!",
        content: data.message,
        color: data.isSuccessed ? "#5b835b" : "#a90329",
        timeout: 2000,
        icon: data.isSuccessed ? "fa fa-check-circle" : "fa fa-warning"
    });
}

function debounce(fn, ms) {
    let timer;

    return function () {
        // Nhận các đối số
        const args = arguments;
        const context = this;

        if (timer) clearTimeout(timer);

        timer = setTimeout(() => {
            fn.apply(context, args);
        }, ms)
    }
}

function resetForm(form) {
    $(".reset[name]", form).each(function () {
        const type = this.type;
        const tag = this.tagName.toLowerCase();
        if (type == 'text' || type == 'password' || tag == 'textarea')
            $(this).val("").trigger("reset")
        else if (type == 'checkbox' || type == 'radio')
            $(this).prop("checked", false)
        else if (tag == 'select')
            $(this).val("").trigger("change")
    })
}