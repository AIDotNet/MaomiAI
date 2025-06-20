/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createBusinessExceptionResponseFromDiscriminatorValue, createEmptyCommandResponseFromDiscriminatorValue, serializeDeletePluginGroupCommand, serializeEmptyCommandResponse, type BusinessExceptionResponse, type DeletePluginGroupCommand, type EmptyCommandResponse } from '../../../../models/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/plugin/{teamId}/delete_group
 */
export interface Delete_groupRequestBuilder extends BaseRequestBuilder<Delete_groupRequestBuilder> {
    /**
     * 删除分组.
     * @param body 删除分组.
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<EmptyCommandResponse>}
     * @throws {BusinessExceptionResponse} error when the service returns a 400 status code
     * @throws {BusinessExceptionResponse} error when the service returns a 401 status code
     * @throws {BusinessExceptionResponse} error when the service returns a 403 status code
     * @throws {BusinessExceptionResponse} error when the service returns a 409 status code
     * @throws {BusinessExceptionResponse} error when the service returns a 500 status code
     */
     delete(body: DeletePluginGroupCommand, requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<EmptyCommandResponse | undefined>;
    /**
     * 删除分组.
     * @param body 删除分组.
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toDeleteRequestInformation(body: DeletePluginGroupCommand, requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const Delete_groupRequestBuilderUriTemplate = "{+baseurl}/api/plugin/{teamId}/delete_group";
/**
 * Metadata for all the requests in the request builder.
 */
export const Delete_groupRequestBuilderRequestsMetadata: RequestsMetadata = {
    delete: {
        uriTemplate: Delete_groupRequestBuilderUriTemplate,
        responseBodyContentType: "application/json",
        errorMappings: {
            400: createBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            401: createBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            403: createBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            409: createBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            500: createBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
        },
        adapterMethodName: "send",
        responseBodyFactory:  createEmptyCommandResponseFromDiscriminatorValue,
        requestBodyContentType: "application/json",
        requestBodySerializer: serializeDeletePluginGroupCommand,
        requestInformationContentSetMethod: "setContentFromParsable",
    },
};
/* tslint:enable */
/* eslint-enable */
