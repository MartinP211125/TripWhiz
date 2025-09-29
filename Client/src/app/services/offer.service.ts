import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';

export enum OfferType {
  Standard = 0,
  Trending = 1,
  Recommended = 2
}

export interface Offer {
  id: string;
  title: string;
  description: string;
  imageUrl: string;
  price: number;
  fromDate: string;
  toDate?: string;
  createdBy?: string;
  accomodationId: string;
}

export interface CreateOfferRequest {
  title: string;
  description: string;
  imageUrl: string;
  createdBy?: string;
  price: number;
  toDate: string;
  fromDate: string;
  accomodationId: string;
  offerType: OfferType;
}

export interface CategorizedOffers {
  recommended: Offer[];
  trending: Offer[];
  standard: Offer[];
}

@Injectable({
  providedIn: 'root'
})
export class OfferService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7089/api/Offer'; 

  getAllOffers(): Observable<Offer[]> {
    return this.http.get<Offer[]>(`${this.apiUrl}`);
  }

  getOfferById(offerId: string): Observable<Offer> {
    return this.http.get<Offer>(`${this.apiUrl}/${offerId}`);
  }

  getPopularOffers(): Observable<Offer[]> {
    return this.http.get<Offer[]>(`${this.apiUrl}/popular`);
  }

  getRecommendedOffers(): Observable<Offer[]> {
    return this.http.get<Offer[]>(`${this.apiUrl}/recommended`);
  }

  createOffer(offer: CreateOfferRequest): Observable<Offer> {
    return this.http.post<Offer>(`${this.apiUrl}`, offer);
  }

  deleteOffer(offerId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${offerId}`);
  }

  // Get all offers categorized by type
  getCategorizedOffers(): Observable<CategorizedOffers> {
    return forkJoin({
      all: this.getAllOffers(),
      recommended: this.getRecommendedOffers(),
      trending: this.getPopularOffers()
    }).pipe(
      map(({ all, recommended, trending }) => {
        console.log('API Response - All:', all?.length || 0); // Debug log
        console.log('API Response - Recommended:', recommended?.length || 0); // Debug log  
        console.log('API Response - Trending:', trending?.length || 0); // Debug log

        // Ensure arrays exist
        const allOffers = all || [];
        const recommendedOffers = recommended || [];
        const trendingOffers = trending || [];

        // Get recommended and trending offer IDs
        const recommendedIds = new Set(recommendedOffers.map(o => o.id));
        const trendingIds = new Set(trendingOffers.map(o => o.id));
        
        // Filter standard offers (not recommended or trending)
        const standard = allOffers.filter(offer => 
          !recommendedIds.has(offer.id) && !trendingIds.has(offer.id)
        );

        const result = {
          recommended: recommendedOffers,
          trending: trendingOffers,
          standard: standard
        };

        console.log('Categorized Result:', result); // Debug log
        return result;
      })
    );
  }
}