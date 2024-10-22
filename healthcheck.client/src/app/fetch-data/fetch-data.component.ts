import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Environment } from '../../environments/environment';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styleUrl: './fetch-data.component.scss'
})
export class FetchDataComponent {
  public forecasts?: WeatherForecast[];

  constructor(private http: HttpClient) {
    http.get<WeatherForecast[]>(`${Environment.baseUrl}api/weatherforecast`).subscribe({
      next: (result) => {
        this.forecasts = result;
      },
      error: (error) => console.error(error),
      complete: () => { }
    });
  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
