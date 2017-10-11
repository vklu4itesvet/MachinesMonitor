import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'fetchdata',
    templateUrl: './fetchdata.component.html',
    styles: ['body {}']
})
export class FetchDataComponent {
    public forecasts: WeatherForecast[];
    columnDefs: any;
    rowData: any;

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.columnDefs = [
            { headerName: "Make", field: "dateFormatted", width: 200 },
            { headerName: "Model", field: "temperatureC", width: 200 },
            { headerName: "Price", field: "temperatureF", width: 200 }
        ];

        this.rowData = [
            { make: "Toyota", model: "Celica", price: 35000 },
            { make: "Ford", model: "Mondeo", price: 32000 },
            { make: "Porsche", model: "Boxter", price: 72000 }
        ]


        http.get(baseUrl + 'api/SampleData/WeatherForecasts').subscribe(result => {
            this.forecasts = result.json() as WeatherForecast[];
        }, error => console.error(error));
    }

    onGridReady(params: any) {
        params.api.sizeColumnsToFit();
    }
}

interface WeatherForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
