/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { CreateRequestBuilderRequestsMetadata, type CreateRequestBuilder } from './create/index.js';
// @ts-ignore
import { DefaultconfigurationRequestBuilderRequestsMetadata, type DefaultconfigurationRequestBuilder } from './defaultconfiguration/index.js';
// @ts-ignore
import { ModellistRequestBuilderRequestsMetadata, type ModellistRequestBuilder } from './modellist/index.js';
// @ts-ignore
import { ProviderlistRequestBuilderRequestsMetadata, type ProviderlistRequestBuilder } from './providerlist/index.js';
// @ts-ignore
import { SetdefaultRequestBuilderRequestsMetadata, type SetdefaultRequestBuilder } from './setdefault/index.js';
// @ts-ignore
import { type UpdateRequestBuilder, UpdateRequestBuilderRequestsMetadata } from './update/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/aimodel/{teamId}
 */
export interface WithTeamItemRequestBuilder extends BaseRequestBuilder<WithTeamItemRequestBuilder> {
    /**
     * The create property
     */
    get create(): CreateRequestBuilder;
    /**
     * The defaultconfiguration property
     */
    get defaultconfiguration(): DefaultconfigurationRequestBuilder;
    /**
     * The modellist property
     */
    get modellist(): ModellistRequestBuilder;
    /**
     * The providerlist property
     */
    get providerlist(): ProviderlistRequestBuilder;
    /**
     * The setdefault property
     */
    get setdefault(): SetdefaultRequestBuilder;
    /**
     * The update property
     */
    get update(): UpdateRequestBuilder;
}
/**
 * Uri template for the request builder.
 */
export const WithTeamItemRequestBuilderUriTemplate = "{+baseurl}/api/aimodel/{teamId}";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const WithTeamItemRequestBuilderNavigationMetadata: Record<Exclude<keyof WithTeamItemRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    create: {
        requestsMetadata: CreateRequestBuilderRequestsMetadata,
    },
    defaultconfiguration: {
        requestsMetadata: DefaultconfigurationRequestBuilderRequestsMetadata,
    },
    modellist: {
        requestsMetadata: ModellistRequestBuilderRequestsMetadata,
    },
    providerlist: {
        requestsMetadata: ProviderlistRequestBuilderRequestsMetadata,
    },
    setdefault: {
        requestsMetadata: SetdefaultRequestBuilderRequestsMetadata,
    },
    update: {
        requestsMetadata: UpdateRequestBuilderRequestsMetadata,
    },
};
/* tslint:enable */
/* eslint-enable */
