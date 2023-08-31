/*  Common Functions */

/*
Functions:

    GetParameterByName - retrieves value of a querystring variable
    GetUsername - retrieve username using activex windows script method
    toProperCase - input string, output proper cased string
    htmlEscape - html encode special characters
    Display File - display file with supplied title in a kendow window
    Call_Service - General ajax call
    AutoComplete - General Kendo autocomplete feature
    getCurrentDate - return today's date in yyyy/mm/dd format
    isBlankOrNull - returns true if blank or null 
    checkBoolean - 1 if checkmark checked, 0 otherwise
    checkText - Return default value if blank or null
    checkComboBox - if there is a current value from combobox return it, otherwise return default value
    getCurrentDate - return today's date in yyyy/mm/dd format   
*/


/*     GetParameterByName - retrieves value of a querystring variable */
function GetParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");

    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);

    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

/* GetUsername - retrieve username using activex windows script method */
function GetUsername() {
    var userName = "";
    var browser = navigator.userAgent.toLowerCase();

    if (browser.indexOf("msie 9") !== -1) {
        var wshshell = new ActiveXObject("wscript.shell");
        var username = wshshell.ExpandEnvironmentStrings("%username%");

        userName = username.replace(".", " ");
    }

    return userName;
}

/* toProperCase - input string, output proper cased string */
function toProperCase(inString) {
    var i, str, lowers, uppers;

    str = inString.replace(/([^\W_]+[^\s-]*) */g, function (txt) {
        return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
    });

    // Certain minor words should be left lowercase unless 
    // they are the first or last words in the string
    lowers = [
        'A', 'An', 'The', 'And', 'But', 'Or', 'For', 'Nor', 'As', 'At',
        'By', 'For', 'From', 'In', 'Into', 'Near', 'Of', 'On', 'Onto', 'To', 'With'
    ];

    for (i = 0; i < lowers.length; i++)
        str = str.replace(new RegExp('\\s' + lowers[i] + '\\s', 'g'),
            function (txt) {
                return txt.toLowerCase();
            });

    // Certain words such as initialisms or acronyms should be left uppercase
    uppers = ['Id', 'Tv'];

    for (i = 0; i < uppers.length; i++)
        str = str.replace(new RegExp('\\b' + uppers[i] + '\\b', 'g'),
            uppers[i].toUpperCase());

    return str;
}

/* htmlEscape - html encode special characters */
function htmlEscape(str) {
    return String(str)
        .replace(/&/g, '&amp;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#39;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;');
}

/* Display File - display file with supplied title in a kendow window */
function DisplayFile(dispfile, title) {

    var window = $("#docviewwindow");

    if (window.length === 0) {
        $("body").append('<div id="docviewwindow"/>');

        window = $("#docviewwindow");
    }

    var filename = dispfile.replace(/\\/g, "&#92;");

    window.kendoWindow({
        modal: true,
        pinned: true,
        resizable: true,
        title: title,
        visible: false,
        width: 1720,
        height: 900,
        iframe: true,
        content: filename
    });

    window.data("kendoWindow").open().center();
}


/* kendo grid */
/*
function KendoGrid(id,serviceURL,service,type,parameterfunction,totalfunction,Model,height,pagesize,changefunction,databoundfunction,Columns,rowtemplate) {

    parameterfunction = parameterfunction ?  this[parameterfunction] : null;

    var dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: "http://" + serviceURL + "/ServiceBus.svc/" + service,
                    dataType: "json",
                    type: type,
                    contentType: "application/json;charset=utf-8"
                },
                parameterMap: parameterfunction
            },
            schema: {
                data: service + 'Result',
                total: function (data) {
                    var count = 0;

                    if (data.GetPartListResult.length > 0) {
                        count = data.GetPartListResult[0]["TotalCount"];
                    }

                    return count;
                },
                type: "json",
                id: "ID",
                cache: false,
                model: Model
            },
            pageSize: 17,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true
        });

    dataSource = data ? data : dataSource;


    $('#' + id).kendoGrid({
        dataSource: ,
        height: 660,
        selectable: "row",
        scrollable: true,
        sortable: true,
        filterable: false,
        pageable: true,
        change: function (arg) {
            window.location = 'partsconfigedit.html?ID=' + this.dataItem(this.select())["ID"] + '&Kit_Id=' + this.dataItem(this.select())["Kit_Id"];
        },
        dataBound: function () {
            var grid = $("#partList");
            var colCount = grid.find('.k-grid-header colgroup > col').length;

            if (grid.data("kendoGrid").dataSource._view.length === 0) {
                grid.find('.k-grid-content tbody').append('<tr class="kendo-data-row"><td colspan="' + colCount + '" style="text-align: center; color: maroon">Your search criteria returned no results</td></tr>');
            }
        },
        columns: Columns,
        rowTemplate:rowtemplate
    });

}

*/
/* Combobox */
function ComboBox(id, service, label, data, cascadefrom, cascadefromfield, parameterfunction) {


    cascadefrom = cascadefrom ? cascadefrom : null;
    cascadefromfield = cascadefromfield ? cascadefromfield : null;
    parameterfunction = parameterfunction ? this[parameterfunction] : null;

    var dataSource = new kendo.data.DataSource({
        transport: {
            read: {
                dataType: "json",
                url: "http://" + serviceURL + "/ServiceBus.svc/" + service
            },
            parameterMap: parameterfunction
        }
    });

    dataSource = data ? data : dataSource;


    $('#' + id).kendoComboBox({
        dataBound: function (e) {
            if (this.value() && this.selectedIndex === -1) {
                this._filterSource({
                    value: this.value(),
                    field: this.options.dataTextField,
                    operator: "contains"
                });
                this.select(0);
                if (this.selectedIndex === -1) {

                    this.text("");
                }
            }
        },
        filter: "startswith",
        delay: 1,
        placeholder: label,
        cascadeFrom: cascadefrom,
        cascadeFromField: cascadefromfield,
        dataTextField: "Name",
        dataValueField: "ID",
        autoBind: false,
        dataSource: dataSource,
        template: '<span style="font-size: 10pt">#: Name #</span>'
    });
}


/* Call_Service - General ajax call  */
function Call_Service(inType, inUrl, inData, inContentType, inDataType, inProcessData, inLoader, inSuccess, inFailure) {
    $.ajax({
        context: this,
        type: inType,
        url: inUrl,
        data: inData,
        contentType: inContentType,
        dataType: inDataType,
        processdata: inProcessData,
        success: this[inSuccess],
        error: this[inFailure]
    });
}

/* AutoComplete - General Kendo autocomplete feature */
function AutoComplete(id, service, label) {
    $('#' + id).kendoAutoComplete({
        dataTextField: "Name",
        dataValueField: "ID",
        placeholder: label,
        dataSource: {
            transport: {
                read: {
                    dataType: "json",
                    url: "http://" + serviceURL + "/ServiceBus.svc/" + service,
                }
            }
        },
        suggest: true,
        template: '<span style="font-size: 10pt">#: Name #</span>'
    }).bind("focus", function () {
        var field = $('#' + id).data("kendoAutoComplete");
        field.list.width(400);
    });
}

/* isBlankOrNull - returns true if blank or null */
function isBlankOrNull(tag) {
    if ($.trim($(tag).val()) === '') {
        return true;
    } else {
        return false;
    }
}

/* checkBoolean - 1 if checkmark checked, 0 otherwise */
function checkBoolean(tag) {
    return $(tag).prop('checked') ? 1 : 0;
}

/* checkText - Return default value if blank or null */
function checkText(tag, defvalue) {
    return isBlankOrNull(tag) ? defvalue : $(tag).val();
}

/* checkComboBox - if there is a current value from combobox return it, otherwise return default value */
function checkComboBox(tag, defvalue) {
    return $.trim($(tag).data("kendoComboBox").value()) === '' ? defvalue : $(tag).data("kendoComboBox").value();
}

/* getCurrentDate - return today's date in yyyy/mm/dd format */
function getCurrentDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!`

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    var today1 = yyyy + '/' + mm + '/' + dd;
    return today1;
}


function GetDocumentsBySopNumber(sopNumber) {
    Call_Service("POST", "http://" + serviceURL + "/ServiceBus.svc/GetDocumentsBySopNumber", "{\"sopNumber\": \"" + sopNumber + "\"}", "application/json; charset=utf-8", "json", true, "#loader", "GetDocumentsBySopNumberSuccess", "displayError");
}

function GetDocumentsBySopNumberSuccess(result) {
    var docresult = result.GetDocumentsBySopNumberResult;
    $('#linkPageCell').html("<table width='100%' border='0'><tr><td colspan='2' width='100%' style='text-align:center;'>Reference Materials</td></tr>" + docresult + "</table>");
    return;
}