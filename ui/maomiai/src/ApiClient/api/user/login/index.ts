/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createBusinessExceptionResponseFromDiscriminatorValue, createLoginCommandResponseFromDiscriminatorValue, serializeLoginCommand, serializeLoginCommandResponse, type BusinessExceptionResponse, type LoginCommand, type LoginCommandResponse } from '../../../models/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/user/login
 */
export interface LoginRequestBuilder extends BaseRequestBuilder<LoginRequestBuilder> {
    /**
     * 用户登录.
     * @param body 登录.
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<LoginCommandResponse>}
     * @throws {BusinessExceptionResponse} error when the service returns a 400 status code
     * @throws {BusinessExceptionResponse} error when the service returns a 401 status code
     * @throws {BusinessExceptionResponse} error when the service returns a 403 status code
     * @throws {BusinessExceptionResponse} error when the service returns a 409 status code
     * @throws {BusinessExceptionResponse} error when the service returns a 500 status code
     */
     post(body: LoginCommand, requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<LoginCommandResponse | undefined>;
    /**
     * 用户登录.
     * @param body 登录.
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toPostRequestInformation(body: LoginCommand, requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const LoginRequestBuilderUriTemplate = "{+baseurl}/api/user/login";
/**
 * Metadata for all the requests in the request builder.
 */
export const LoginRequestBuilderRequestsMetadata: RequestsMetadata = {
    post: {
        uriTemplate: LoginRequestBuilderUriTemplate,
        responseBodyContentType: "application/json",
        errorMappings: {
            400: createBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            401: createBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            403: createBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            409: createBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            500: createBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
        },
        adapterMethodName: "send",
        responseBodyFactory:  createLoginCommandResponseFromDiscriminatorValue,
        requestBodyContentType: "application/json",
        requestBodySerializer: serializeLoginCommand,
        requestInformationContentSetMethod: "setContentFromParsable",
    },
};
/* tslint:enable */
/* eslint-enable */
