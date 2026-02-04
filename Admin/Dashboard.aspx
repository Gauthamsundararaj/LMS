<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Admin.Dashboard" %>

<%@ Register Src="../Controls/Header.ascx" TagPrefix="uc1" TagName="Header" %>
<%@ Register Src="../Controls/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>LMS Admin Dashboard</title>
    <link href="../assets/css/CustomPagination.css" rel="stylesheet" />
    <style>
        .summary-card {
            border-radius: 12px;
            color: #333;
            min-height: 120px;
            cursor: pointer;
            transition: transform 0.25s ease, box-shadow 0.25s ease;
        }

            .summary-card:hover {
                transform: scale(1.04);
                box-shadow: 0 10px 20px rgba(0, 0, 0, 0.15);
            }

        .icon {
            width: 70px;
            height: 70px;
            object-fit: contain;
        }

        .bg-NewAddBooks {
            background: linear-gradient(135deg, #4a90e2, #d6e9ff);
        }

        .bg-issued {
            background: linear-gradient(135deg, #43a047, #dcedc8);
        }

        .bg-due {
            background: linear-gradient(135deg, #ef6c00, #ffe0b2);
        }

        .bg-returned {
            background: linear-gradient(135deg, #c2185b, #f8bbd0);
        }

        .chart-card {
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0,0,0,.08);
        }

        .google-visualization-legend text {
            font-size: 13px !important;
        }

        .card {
            letter-spacing: 0px !important;
        }

        .csv-download-btn {
            position: absolute;
            right: 55px; /* stays fixed from right */
            top: 50%;
            transform: translateY(-50%);
        }

        .modal-header .btn-close {
            top: 15px !important;
        }
    </style>
</head>

<body>
    <uc1:header runat="server" id="Header" />

    <form id="form1" runat="server">
        <div class="container-fluid pt-3">

            <!-- FILTERS -->
            <div class="row mb-3 align-items-center">
                <div class="col-md-5">
                    <h2 class="fw-bold">Library Dashboard</h2>
                </div>

                <div class="col-md-7 text-end">
                    <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" CssClass="form-control d-inline w-25"     onkeydown="return false;"  onpaste="return false;"/>
                    <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" CssClass="form-control d-inline w-25 ms-2"  onkeydown="return false;"  onpaste="return false;"/>
                 
                    <asp:Button ID="btnSearch" runat="server" Text="Search"
                        CssClass="btn btn-primary ms-2"
                        OnClick="btnSearch_Click" />
                </div>
            </div>

            <!-- SUMMARY CARDS -->
            <div class="row mb-0">

                <!-- TOTAL BOOKS -->
                <div class="col-lg-3 col-md-6 ">
                    <asp:LinkButton runat="server" CssClass="text-decoration-none d-block"
                        CommandArgument="Added"
                        OnCommand="Card_Click">
                        <div class="card summary-card bg-NewAddBooks">
                            <div class="card-body d-flex justify-content-between align-items-center">
                                <div>
                                    <h5>Added Books</h5>
                                    <h2>
                                        <asp:Label ID="lblTotalBooks" runat="server" Text="0" /></h2>
                                </div>
                                <img src="../assets/images/icons/NewBook.png" class="icon" />
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>

                <!-- ISSUED -->
                <div class="col-lg-3 col-md-6 ">
                    <asp:LinkButton runat="server" CssClass="text-decoration-none d-block"
                        CommandArgument="Issued"
                        OnCommand="Card_Click">
                        <div class="card summary-card bg-issued">
                            <div class="card-body d-flex justify-content-between align-items-center">
                                <div>
                                    <h5>Issued Books</h5>
                                    <h2>
                                        <asp:Label ID="lblTotalIssued" runat="server" Text="0" /></h2>
                                </div>
                                <img src="../assets/images/icons/issuebooks.png" class="icon" />
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>
                <!-- DUE -->
                <div class="col-lg-3 col-md-6 ">
                    <asp:LinkButton runat="server" CssClass="text-decoration-none d-block"
                        CommandArgument="Due"
                        OnCommand="Card_Click">
                        <div class="card summary-card bg-due">
                            <div class="card-body d-flex justify-content-between align-items-center">
                                <div>
                                    <h5>Due Books</h5>
                                    <h2>
                                        <asp:Label ID="lblDueBooks" runat="server" Text="0" /></h2>
                                </div>
                                <img src="../assets/images/icons/deadline.png" class="icon" />
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>

                <!-- RETURNED -->
                <div class="col-lg-3 col-md-6">
                    <asp:LinkButton runat="server" CssClass="text-decoration-none d-block"
                        CommandArgument="Returned"
                        OnCommand="Card_Click">
                        <div class="card summary-card bg-returned">
                            <div class="card-body d-flex justify-content-between align-items-center">
                                <div>
                                    <h5>Returned Books</h5>
                                    <h2>
                                        <asp:Label ID="lblReturnedBooks" runat="server" Text="0" /></h2>
                                </div>
                                <img src="../assets/images/icons/book.png" class="icon" />
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>

            </div>

            <!-- CHARTS -->
            <div class="row">

                <div class="col-md-6 mb-4">
                    <div class="card chart-card">
                        <div class="card-header fw-bold">Issued vs Returned</div>
                        <div class="card-body">
                            <div id="barChart" style="height: 350px;"></div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="card chart-card">
                        <div class="card-header">
                            <h5>Issued vs Returned Distribution</h5>
                        </div>
                        <div class="card-body text-center">
                            <canvas id="doubleRingDonut" style="max-height: 350px;"></canvas>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="card chart-card">
                        <div class="card-header">
                            <h5>Issue vs Returned Day Wise</h5>
                        </div>
                        <div class="card-body text-center">
                            <canvas id="dayWiseChart" height="220"></canvas>
                        </div>
                    </div>
                </div>
            </div>

            <!-- MODAL GRID -->
            <div class="modal fade" id="dataModal" tabindex="-1">
                <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header bg-primary text-white p-2 position-relative">
                            <h5 class="modal-title mb-0">
                                <asp:Label ID="lblModalTitle" runat="server" />
                            </h5>

                            <!-- Download CSV -->
                            <asp:LinkButton
                                ID="LinkButton1"
                                runat="server"
                                OnClick="btnDownload_Click"
                                ToolTip="Download CSV"
                                CssClass="csv-download-btn">
                                <asp:Image
                                    ID="Image1"
                                    runat="server"
                                    ImageUrl="~/assets/images/icons/csvdownload.png"
                                    AlternateText="Download"
                                    Width="35"
                                    Height="35" />
                            </asp:LinkButton>
                            <!-- Close button -->
                            <button type="button"
                                class="btn-close btn-close-white ms-2 "
                                data-bs-dismiss="modal"
                                aria-label="Close">
                            </button>
                        </div>

                        <div class="modal-body pt-0">
                            <div class="col-12 col-lg-auto ms-lg-auto text-lg-end pt-0">
                                <asp:Label ID="lblRecordCount" runat="server"
                                    CssClass="fw-bold text-primary"></asp:Label>
                            </div>
                            <asp:GridView ID="gvData" runat="server"
                                CssClass="table table-bordered table-striped table-hover fixed-header"
                                AutoGenerateColumns="true"
                                EmptyDataText="No records found"
                                AllowPaging="True" PageSize="5"
                                PagerSettings-Visible="false">
                            </asp:GridView>



                            <div class="pager-fixed">
                                <asp:Repeater ID="rptPager" runat="server" OnItemCommand="rptPager_ItemCommand">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPage" runat="server"
                                            CssClass='<%# (bool)Eval("IsActive") ? "page-btn active" : "page-btn" %>'
                                            CommandName='<%# Eval("Command") %>'
                                            CommandArgument='<%# Eval("PageIndex") %>'
                                            Enabled='<%# Eval("Enabled") %>'
                                            Text='<%# Eval("Text") %>'>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- HIDDEN FIELDS -->
        <asp:HiddenField ID="hfStudentIssued" runat="server" />
        <asp:HiddenField ID="hfStudentReturned" runat="server" />
        <asp:HiddenField ID="hfStaffIssued" runat="server" />
        <asp:HiddenField ID="hfStaffReturned" runat="server" />

        <asp:HiddenField ID="hfStudentTotal" runat="server" />
        <asp:HiddenField ID="hfStaffTotal" runat="server" />

        <asp:HiddenField ID="hfStudentIssuedDD" runat="server" />
        <asp:HiddenField ID="hfStudentReturnedDD" runat="server" />
        <asp:HiddenField ID="hfStaffIssuedDD" runat="server" />
        <asp:HiddenField ID="hfStaffReturnedDD" runat="server" />
        <asp:HiddenField ID="hfDayLabels" runat="server" />
        <asp:HiddenField ID="hfDayIssued" runat="server" />
        <asp:HiddenField ID="hfDayReturned" runat="server" />


    </form>

    <uc1:footer runat="server" id="Footer" />

    <!-- GOOGLE CHARTS -->
    <script src="../assets/js/chart/google/google-chart-loader.js"></script>
    <script src="../assets/js/customcharts/chartjs.js"></script>
    <script src="../assets/js/customcharts/chartjsplugin.js"></script>
    <script>
        google.charts.load('current', { packages: ['corechart', 'bar'] });
        google.charts.setOnLoadCallback(drawAllCharts);

        function safeParse(id) {
            var val = document.getElementById(id).value;
            return val ? parseInt(val) : 0;
        }

        function drawAllCharts() {

            drawBarChart();

        }

        function drawBarChart() {

            var data = google.visualization.arrayToDataTable([
                ['Category', 'Issued', 'Returned'],
                ['Student', safeParse('<%=hfStudentIssued.ClientID%>'), safeParse('<%=hfStudentReturned.ClientID%>')],
                ['Staff', safeParse('<%=hfStaffIssued.ClientID%>'), safeParse('<%=hfStaffReturned.ClientID%>')]
            ]);

            var options = {
                bars: 'vertical',
                height: 350,
                legend: { position: 'top' },
                chartArea: { width: '80%', height: '70%' },
                colors: ['#4a90e2', '#ef6c00'],
                animation: { startup: true, duration: 700 }
            };

            var chart = new google.charts.Bar(document.getElementById('barChart'));
            chart.draw(data, google.charts.Bar.convertOptions(options));
        }

        document.addEventListener("DOMContentLoaded", function () {

            const studentIssued = +document.getElementById('<%=hfStudentIssuedDD.ClientID%>').value || 0;
            const studentReturned = +document.getElementById('<%=hfStudentReturnedDD.ClientID%>').value || 0;
            const staffIssued = +document.getElementById('<%=hfStaffIssuedDD.ClientID%>').value || 0;
            const staffReturned = +document.getElementById('<%=hfStaffReturnedDD.ClientID%>').value || 0;

            new Chart(document.getElementById('doubleRingDonut'), {
                type: 'doughnut',
                data: {
                    labels: ['Issued', 'Returned'],
                    datasets: [
                        {
                            label: 'Student Issued',
                            data: [studentIssued, studentReturned],
                            backgroundColor: ['#4a90e2', '#43a047'],
                            borderWidth: 5,
                            weight: 13
                        },
                        {
                            label: 'Staff Issued',
                            data: [staffIssued, staffReturned],
                            backgroundColor: ['#f6c23e', '#e74a3b'],
                            borderWidth: 5,
                            weight: 10
                        }
                    ]
                },
                options: {
                    cutout: '55%',
                    plugins: {
                        legend: {
                            position: 'bottom',
                            labels: {
                                generateLabels: function () {
                                    return [
                                        { text: 'Student Issued', fillStyle: '#4a90e2' },
                                        { text: 'Student Returned', fillStyle: '#43a047' },
                                        { text: 'Staff Issued', fillStyle: '#f6c23e' },
                                        { text: 'Staff Returned', fillStyle: '#e74a3b' }
                                    ];
                                }
                            }
                        },
                        datalabels: {
                            color: '#fff',
                            font: { size: 12 },
                            formatter: v => v > 0 ? v : ''
                        },
                        tooltip: {
                            callbacks: {
                                label: function (ctx) {
                                    const userType = ctx.datasetIndex === 0 ? 'Student' : 'Staff';
                                    const status = ctx.label; // Issued / Returned
                                    return `${userType} ${status}: ${ctx.raw}`;
                                }
                            }
                        }
                    }
                },
                plugins: [ChartDataLabels]
            });
        });


        const labels = $('#<%=hfDayLabels.ClientID%>').val().split(',');
        const issued = $('#<%=hfDayIssued.ClientID%>').val().split(',').map(Number);
        const returned = $('#<%=hfDayReturned.ClientID%>').val().split(',').map(Number);

        new Chart(document.getElementById('dayWiseChart'), {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [
                    {
                        label: 'Issued',
                        data: issued,
                        backgroundColor: '#1f77b4'
                    },
                    {
                        label: 'Returned',
                        data: returned,
                        backgroundColor: '#ff7f0e'
                    }
                ]
            },
            options: {
                scales: {
                    x: { stacked: true },
                    y: { stacked: true, beginAtZero: true }
                },
                plugins: {
                    legend: { position: 'bottom' }
                }
            }
        });


    </script>

</body>
</html>





