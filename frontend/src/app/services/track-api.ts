import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TrackListItem, TrackDetail } from '../models/track.model';

@Injectable({
  providedIn: 'root'
})
export class TrackApi {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5080/api/tracks';

  // Implement getTracks() here.
    getTracks(status?: string): Observable<TrackListItem[]> {
        return this.http.get<TrackListItem[]>(this.apiUrl, {
            params: status ? { status } : {}
        });
    }
  // Implement getTrackById() here.
  getTrackById(id: number): Observable<TrackDetail> {
    const url = `${this.apiUrl}/${id}`;
    return this.http.get<TrackDetail>(url);
  }
}