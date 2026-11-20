
export interface TrackListItem {
  id: number;
  title: string;
  artistId: number;
  artistName: string;
  genre: string;
  status: string;
}

export interface TrackDistribution {
  dspId: number;
  dspName: string;
  submittedAt: string;
  status: string;
}

export interface TrackDetail extends TrackListItem {
  isrc: string;
  releaseDate: string;
  distributions: TrackDistribution[];
}
