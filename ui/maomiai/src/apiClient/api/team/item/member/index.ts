/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { InviteRequestBuilderRequestsMetadata, type InviteRequestBuilder } from './invite/index.js';
// @ts-ignore
import { RemoveRequestBuilderRequestsMetadata, type RemoveRequestBuilder } from './remove/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/team/{teamId}/member
 */
export interface MemberRequestBuilder extends BaseRequestBuilder<MemberRequestBuilder> {
    /**
     * The invite property
     */
    get invite(): InviteRequestBuilder;
    /**
     * The remove property
     */
    get remove(): RemoveRequestBuilder;
}
/**
 * Uri template for the request builder.
 */
export const MemberRequestBuilderUriTemplate = "{+baseurl}/api/team/{teamId}/member";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const MemberRequestBuilderNavigationMetadata: Record<Exclude<keyof MemberRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    invite: {
        requestsMetadata: InviteRequestBuilderRequestsMetadata,
    },
    remove: {
        requestsMetadata: RemoveRequestBuilderRequestsMetadata,
    },
};
/* tslint:enable */
/* eslint-enable */
