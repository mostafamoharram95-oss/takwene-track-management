import { Routes } from '@angular/router';
import { TrackList } from './pages/track-list/track-list';
import { TrackDetail } from './pages/track-detail/track-detail';

export const routes: Routes = [
  { path: '', component: TrackList },
  { path: 'tracks/:id', component: TrackDetail },
  { path: '**', redirectTo: '' }
];