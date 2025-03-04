

const columnDefs = [
    { field: 'bookID', headerName: 'BookID', cellDataType: 'number', sortable: true, filter: 'agNumberColumnFilter' },
    { field: 'title', flex: 2, headerName: 'Title' },
    { field: 'isbn', headerName: 'ISBN' },
    { field: 'vendorName', headerName: 'Vendor' },
    { field: 'salesDate', headerName: 'Sales Date' },
    { field: 'unitsSold', headerName: 'Units Sold', cellDataType: 'number', sortable: true, filter: 'agNumberColumnFilter' },
    { field: 'unitsToDate', headerName: 'Units To Date', cellDataType: 'number', filter: 'agNumberColumnFilter' },
    { field: 'salesThisPeriod', headerName: 'Sales Period', cellDataType: 'number', filter: 'agNumberColumnFilter', valueFormatter: params => currencyFormatter(params.data.salesThisPeriod) },
    { field: 'salesToDate', headerName: 'Sales To Date', cellDataType: 'number', filter: 'agNumberColumnFilter', valueFormatter: params => currencyFormatter(params.data.salesToDate) },
    { field: 'royalty', headerName: 'Royalty', cellDataType: 'number', filter: 'agNumberColumnFilter', valueFormatter: params => currencyFormatter(params.data.royalty) }

];

const gridOptions = {
    columnDefs: columnDefs,
    pagination: true,
    paginationPageSize: 20,
    rowHeight: 32.5,
    defaultColDef: {
        filter: "agTextColumnFilter",
        floatingFilter: true,
        editable: false,
        sortable: true,
        flex: 1
    },
    enableCharts: true,
    enableRangeSelection: true,

    // Enable server-side sorting and filtering
    rowModelType: 'serverSide',

    // Datasource definition
    serverSideDatasource: {
        getRows: (params) => {
            const { startRow, endRow, sortModel } = params.request;
            let sortColumn = "Title";
            let sortDirection = "ASC";
            let filter = JSON.stringify(params.request.filterModel);

            // Extract sorting information
            if (sortModel.length > 0) {
                sortColumn = sortModel[0].colId;
                sortDirection = sortModel[0].sort;
            }
            sortColumn = sortColumn === "null" ? "Title" : sortColumn;
            sortDirection = sortDirection === "asc" ? "ASC" : "DESC";

            // Build your API request URL
            //debugger;
            const url = API_URL + `sale/gridsearch?startRow=${startRow}&endRow=${endRow}&sortColumn=${sortColumn}&sortDirection=${sortDirection}&filter=${filter}`;
            fetch(url)
                .then(response => response.json())
                .then(data => {
                    params.success({ rowData: data, rowCount: data.length });  // Adjust 'totalRecords' if your API provides it
                })
                .catch(error => {
                    console.error("Error fetching data:", error);
                    params.fail();
                });
        }
    }

};

// Create the grid instance
gridApi = new agGrid.createGrid(document.getElementById('myGrid'), gridOptions);


var savedFilterModel = null;

function clearFilters() {
    gridApi.setFilterModel(null);
}

function saveFilterModel() {

    savedFilterModel = gridApi.getFilterModel();

    var keys = Object.keys(savedFilterModel);
    var savedFilters = keys.length > 0 ? keys.join(", ") : "(none)";

    document.querySelector("#savedFilters").textContent = savedFilters;
}

function restoreFilterModel() {
    gridApi.setFilterModel(savedFilterModel);
}

function destroyFilter() {
    // gridApi.destroyFilter("athlete");
    gridApi.setFilterModel(null);

    document.querySelector("#savedFilters").textContent = '';
}
function currencyFormatter(currency) {
    if (currency === undefined) {
        return "$0";
    }
    else {
        var sansDec = currency.toFixed(2);
        return "$" + `${sansDec}`;
    }
}
