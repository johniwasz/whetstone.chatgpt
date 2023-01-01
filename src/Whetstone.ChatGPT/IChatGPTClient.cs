using Whetstone.ChatGPT.Models;

namespace Whetstone.ChatGPT
{

    /// <summary>
    /// A client for the OpenAI GPT-3 API.
    /// </summary>
    public interface IChatGPTClient : IDisposable
    {
        /// <summary>
        /// Creates a completion for the provided prompt and parameters
        /// </summary>
        /// <remarks>
        /// <para>See <seealso cref="https://beta.openai.com/docs/api-reference/completions/create">Create completion</seealso>.</para>
        /// </remarks>
        /// <param name="completionRequest">A well-defined prompt for requesting a completion.</param>
        /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
        /// <returns>A completion populated by the GPT-3 API.</returns>
        /// <exception cref="ArgumentNullException">completionRequest is required.</exception>
        /// <exception cref="ArgumentException">Model is required.</exception>
        /// <returns><see cref="ChatGPTModelsResponse"/>Exception generated while processing request.</returns>
        Task<ChatGPTCompletionResponse?> CreateCompletionAsync(ChatGPTCompletionRequest completionRequest, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Given a prompt and an instruction, the model will return an edited version of the prompt.
        /// </summary>
        /// <remarks>
        /// <para>If <c>Model</c> is not provided, the default of "text-davinci-edit-001" is used. <see cref="ChatGPTEditModels.Davinci">Davinci</see></para>
        /// <para>An example of using this would be to fix any spelling mistakes in the prompt by setting <c>Instruction</c> to "Fix the spelling mistakes".</para>
        /// <para>See <seealso cref="https://beta.openai.com/docs/api-reference/edits/create">Create Edits</seealso></para>
        /// </remarks>
        /// <param name="createEditRequest">Submit an instruction to edit a propmpt.</param>
        /// <param name="cancellationToken">Optional. Propagates notifications that opertions should be cancelled.</param>
        /// <returns>Response to the <c>CreateEditRequest</c> request.</returns>
        /// <exception cref="ArgumentException"><c>CreateEditRequest</c> requires an <c>Instruction</c></exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTCreateEditResponse?> CreateEditAsync(ChatGPTCreateEditRequest createEditRequest, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Delete a file.
        /// </summary>
        /// <param name="fileId">Id of the file to delete</param>
        /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
        /// <returns>Confirmation the file was deleted.</returns>
        /// <exception cref="ArgumentException">fileId cannot be null or whitespace</exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTDeleteResponse?> DeleteFileAsync(string? fileId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Returns a list of files that belong to the user's organization.
        /// </summary>
        /// <remarks>
        /// <para>See <seealso cref="https://beta.openai.com/docs/api-reference/files/list">List Files</seealso>.</para>
        /// </remarks>
        /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
        /// <returns><see cref="ChatGPTFileInfo">A list of available models.</see></returns>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTListResponse<ChatGPTFileInfo>?> ListFilesAsync(CancellationToken? cancellationToken = null);


        /// <summary>
        /// Lists the currently available models, and provides basic information about each one such as the owner and availability.
        /// </summary>
        /// <remarks>
        /// <para>For a recommended list of models see <see cref="ChatGPTCompletionModels">ChatGPTCompletionModels</see></para>
        /// <para>See <seealso cref="https://beta.openai.com/docs/api-reference/models/list">List Models</seealso>.</para>
        /// </remarks>
        /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
        /// <returns><see cref="ChatGPTModel">A list of available models.</see></returns>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTListResponse<ChatGPTModel>?> ListModelsAsync(CancellationToken? cancellationToken = null);



        /// <summary>
        /// Returns information about a specific file.
        /// </summary>
        /// <param name="fileId">Id of the file to retrieve for its info.</param>
        /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
        /// <returns>Confirmation the file was deleted.</returns>
        /// <exception cref="ArgumentException">fileId cannot be null or whitespace</exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTFileInfo?> RetrieveFileAsync(string? fileId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Returns the contents of the specified file
        /// </summary>
        /// <param name="fileId">The ID of the file to use for this request</param>
        /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        Task<ChatGPTFileContent?> RetrieveFileContentAsync(string? fileId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Retrieves a model instance, providing basic information about the model such as the owner and permissioning.
        /// </summary>
        /// <remarks>
        /// <para>See <seealso cref="https://beta.openai.com/docs/api-reference/models/retrieve">Retrieve Models</seealso>.</para>
        /// </remarks>
        /// <param name="modelId">The ID of the model to use for this request. See <see cref="ChatGPTCompletionModels">ChatGPTCompletionModels</see></param>
        /// <param name="cancellationToken">Optional. Propagates notifications that opertions should be cancelled.</param>
        /// <returns>A generated completion.</returns>
        /// <exception cref="ArgumentException">modelId is required.</exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTModel?> RetrieveModelAsync(string modelId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Upload a file that contains document(s) to be used across various endpoints/features. Currently, the size of all the files uploaded by one organization can be up to 1 GB. Please contact us if you need to increase the storage limit.
        /// </summary>
        /// <param name="fileRequest">Defines the file name, contents, and purpose.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">File contents and Purpose are required</exception>
        /// <exception cref="ArgumentException">File contents and Purpose are required</exception>
        Task<ChatGPTFileInfo?> UploadFileAsync(ChatGPTUploadFileRequest? fileRequest, CancellationToken? cancellationToken = null);
    }
}