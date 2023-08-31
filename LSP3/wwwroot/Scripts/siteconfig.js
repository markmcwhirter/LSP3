var take = 20;
var skip = 0;
var page = 1;
var pageSize = 20;


jQuery(document).ready(function () {

    jQuery.support.cors = true;

    var Controller = "BulkSales";
    var serviceURL = "localhost:5001";



    switch (Controller) {
        case "BulkSales":
            {
                var Columns = [
                    { field: "SaleID", title: "SaleID" },
                    { field: "BookID", title: "BookID" },
                    { field: "VendorID", title: "VendorID" },
                    { field: "SalesDate", title: "Sales Date" },
                    { field: "UnitsSold", title: "UnitsSold" },
                    { field: "Royalty", title: "Royalty" },
                    { field: "SalesToDate", title: "SalesToDate" },
                    { field: "UnitsToDate", title: "UnitsToDate" },
                    { field: "SalesThisPeriod", title: "SalesThisPeriod" },
                    { command: ["edit", "destroy"], title: "&nbsp;", width: "200px" }
                ];

                var Model = ({
                    id: "SaleID",
                    fields: {
                        SaleID: { type: "number" },
                        BookID: { type: "number" },
                        VendorID: { type: "number" },
                        SalesDate: { type: "string" },
                        UnitsSold: { type: "number" },
                        Royalty: { type: "number" },
                        DateCreated: { type: "string" },
                        DateUpdated: { type: "date" },
                        SalesToDate: { type: "string" },
                        UnitsToDate: { type: "number" },
                        SalesThisPeriod: { type: "number" }
                    }
                });

                var DataSource = new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "http://" + serviceURL + "/ServiceBus.svc/ReadBulkSales",
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json;charset=utf-8"
                        },
                        update: {
                            url: "http://" + serviceURL + "/ServiceBus.svc/UpdateBulkSales",
                            dataType: "json",
                            cache: false,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            complete: function (e) {
                                $("#Grid").data("kendoGrid").dataSource.read();
                            }
                        },
                        destroy: {
                            url: "http://" + serviceURL + "/ServiceBus.svc/DeleteBulkSales",
                            dataType: "json",
                            cache: false,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            complete: function (e) {
                                $("#Grid").data("kendoGrid").dataSource.read();
                            }
                        },
                        create: {
                            url: "http://" + serviceURL + "/ServiceBus.svc/CreateBulkSales",
                            dataType: "json",
                            cache: false,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            complete: function (e) {
                                $("#Grid").data("kendoGrid").dataSource.read();
                            }
                        },
                        parameterMap: function (options, operation) {
                            
                            if (operation !== "read" && options.models) {
                                return { models: kendo.stringify(options.models) };
                            } else {
                                return kendo.stringify(options);
                            }
                        }
                    },
                    pageSize: 14,
                    serverPaging: true,
                    serverFiltering: true,
                    serverSorting: true,
                    schema: {
                        data: "ReadBulkSalesResult",
                        total: function (e) {
                            if (!jQuery.isEmptyObject(e.ReadBulkSalesResult)) {
                                return e.ReadBulkSalesResult[0].TotalCount;
                            }
                            else {
                                return 0;
                            }
                        },
                        model: Model,
                        errors: "Errors",
                        error: function (e) {
                            alert("Delete Failed");
                            this.cancelChanges();
                        }
                    }
                });
                var lookUpMessage = "Sales";
                var tblMessage = "Create New Sales Entry";
                toolbar = [{ name: "create", text: tblMessage }];
                var headerMessage = "Sales Report";

                abort = false;
                break;
            }
            break;
    }
    $("#Grid").kendoGrid({
        dataSource: DataSource,
        height: 658,
        sortable: true,
        selectable: "row",
        toolbar: toolbar,
        columns: Columns,
        pageable: true,
        scrollable: false,
        editable: "inline",
        save: function () {
            this.refresh();
        }
    });
});