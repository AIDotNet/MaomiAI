/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createBusinessExceptionResponseFromDiscriminatorValue, createEmptyCommandResponseFromDiscriminatorValue, serializeEmptyCommandResponse, serializeUploadtUserAvatarCommand, type BusinessExceptionResponse, type EmptyCommandResponse, type UploadtUserAvatarCommand } from '../../../models/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/user/uploadavatar
 */
export interface UploadavatarRequestBuilder extends BaseRequestBuilder<UploadavatarRequestBuilder> {
    /**
     * 结束上传头像.
     * @param body 上传用户头像.
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<EmptyCommandResponse>}
     * @throws {BusinessExceptionResponse} error when the service returns a 400 status code
     * @throws {BusinessExceptionResponse} error when the service returns a 401 status code
     * @throws {BusinessExceptionResponse} error when the service returns a 403 status code
     * @throws {BusinessExceptionResponse} error when the service returns a 409 status code
     * @throws {BusinessExceptionResponse} error when the service returns a 500 status code
     */
     post(body: UploadtUserAvatarCommand, requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<EmptyCommandResponse | undefined>;
    /**
     * 结束上传头像.
     * @param body 上传用户头像.
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toPostRequestInformation(body: UploadtUserAvatarCommand, requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const UploadavatarRequestBuilderUriTemplate = "{+baseurl}/api/user/uploadavatar";
/**
 * Metadata for all the requests in the request builder.
 */
export const UploadavatarRequestBuilderRequestsMetadata: RequestsMetadata = {
    post: {
        uriTemplate: UploadavatarRequestBuilderUriTemplate,
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
        requestBodySerializer: serializeUploadtUserAvatarCommand,
        requestInformationContentSetMethod: "setContentFromParsable",
    },
};
/* tslint:enable */
/* eslint-enable */
