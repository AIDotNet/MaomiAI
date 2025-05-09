/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { Check_nameRequestBuilderRequestsMetadata, type Check_nameRequestBuilder } from './check_name/index.js';
// @ts-ignore
import { ConfigRequestBuilderNavigationMetadata, type ConfigRequestBuilder } from './config/index.js';
// @ts-ignore
import { CreateRequestBuilderRequestsMetadata, type CreateRequestBuilder } from './create/index.js';
// @ts-ignore
import { ItemRequestBuilderNavigationMetadata, type ItemRequestBuilder } from './item/index.js';
// @ts-ignore
import { Joined_listRequestBuilderRequestsMetadata, type Joined_listRequestBuilder } from './joined_list/index.js';
// @ts-ignore
import { MemberRequestBuilderNavigationMetadata, type MemberRequestBuilder } from './member/index.js';
// @ts-ignore
import { SetadminRequestBuilderRequestsMetadata, type SetadminRequestBuilder } from './setadmin/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type Guid, type KeysToExcludeForNavigationMetadata, type NavigationMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/team
 */
export interface TeamRequestBuilder extends BaseRequestBuilder<TeamRequestBuilder> {
    /**
     * The check_name property
     */
    get check_name(): Check_nameRequestBuilder;
    /**
     * The config property
     */
    get config(): ConfigRequestBuilder;
    /**
     * The create property
     */
    get create(): CreateRequestBuilder;
    /**
     * The joined_list property
     */
    get joined_list(): Joined_listRequestBuilder;
    /**
     * The member property
     */
    get member(): MemberRequestBuilder;
    /**
     * The setadmin property
     */
    get setadmin(): SetadminRequestBuilder;
    /**
     * Gets an item from the ApiSdk.api.team.item collection
     * @param id 团队ID.
     * @returns {ItemRequestBuilder}
     */
     byId(id: Guid) : ItemRequestBuilder;
}
/**
 * Uri template for the request builder.
 */
export const TeamRequestBuilderUriTemplate = "{+baseurl}/api/team";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const TeamRequestBuilderNavigationMetadata: Record<Exclude<keyof TeamRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    byId: {
        navigationMetadata: ItemRequestBuilderNavigationMetadata,
        pathParametersMappings: ["%2Did"],
    },
    check_name: {
        requestsMetadata: Check_nameRequestBuilderRequestsMetadata,
    },
    config: {
        navigationMetadata: ConfigRequestBuilderNavigationMetadata,
    },
    create: {
        requestsMetadata: CreateRequestBuilderRequestsMetadata,
    },
    joined_list: {
        requestsMetadata: Joined_listRequestBuilderRequestsMetadata,
    },
    member: {
        navigationMetadata: MemberRequestBuilderNavigationMetadata,
    },
    setadmin: {
        requestsMetadata: SetadminRequestBuilderRequestsMetadata,
    },
};
/* tslint:enable */
/* eslint-enable */
