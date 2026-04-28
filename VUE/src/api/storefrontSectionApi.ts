import { httpClient } from '../service/httpClient';

export interface StorefrontSection {
    storefrontSectionId?: number;
    slug: string;
    category: string;
    title: string;
    description: string;
    featuredEyebrow: string;
    allEyebrow: string;
    allTitle: string;
    allDescription: string;
    emptyTitle: string;
    emptyDescription: string;
    updatedDate?: string;
}

export type StorefrontSectionPayload = Omit<StorefrontSection, 'storefrontSectionId' | 'updatedDate' | 'slug'>;

export const fetchAdminStorefrontSection = async (slug: string) => {
    const response = await httpClient.get<StorefrontSection>(`storefrontsections/${encodeURIComponent(slug)}`);
    return response.data;
}

export const updateAdminStorefrontSection = async (slug: string, payload: StorefrontSectionPayload) => {
    const response = await httpClient.put<StorefrontSection>(`storefrontsections/${encodeURIComponent(slug)}`, payload);
    return response.data;
}