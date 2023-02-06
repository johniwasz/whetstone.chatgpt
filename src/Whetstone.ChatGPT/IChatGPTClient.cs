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
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
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
        /// <returns>File into for the given fileId.</returns>
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
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
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
        /// Delete a fine-tuned model. You must have the Owner role in your organization.
        /// </summary>
        /// <param name="modelId">Id of the model to delete</param>
        /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
        /// <returns>Confirmation the file was deleted.</returns>
        /// <exception cref="ArgumentException">modelId cannot be null or whitespace</exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTDeleteResponse?> DeleteModelAsync(string? modelId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Upload a file that contains document(s) to be used across various endpoints/features. Currently, the size of all the files uploaded by one organization can be up to 1 GB. Please contact us if you need to increase the storage limit.
        /// </summary>
        /// <param name="fileRequest">Defines the file name, contents, and purpose.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">File contents and Purpose are required</exception>
        /// <exception cref="ArgumentException">File contents and Purpose are required</exception>
        Task<ChatGPTFileInfo?> UploadFileAsync(ChatGPTUploadFileRequest? fileRequest, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Creates a job that fine-tunes a specified model from a given dataset.
        /// </summary>
        /// <remarks>
        /// <para>Response includes details of the enqueued job including job status and the name of the fine-tuned models once complete.</para>
        /// <para><see href="https://beta.openai.com/docs/guides/fine-tuning"/>Learn more about Fine-tuning.</para>
        /// <para>See <seealso href="https://beta.openai.com/docs/api-reference/fine-tunes/create">Create fine-tune</seealso></para>
        /// </remarks>
        /// <param name="createFineTuneRequest">A fine tuning request that requires a TrainingFileId. Model defalts to <see cref="ChatGPTFineTuneModels.Ada">Ada</see>.</param>
        /// <param name="cancellationToken">Optional. Propagates notifications that opertions should be cancelled.</param>
        /// <returns>A fine tune reponse indicating that processing has started.</returns>
        /// <exception cref="ArgumentNullException">createFineTuneRequest cannot be null</exception>
        /// <exception cref="ArgumentException">TrainingFileId cannot be null or empty</exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTFineTuneJob?> CreateFineTuneAsync(ChatGPTCreateFineTuneRequest? createFineTuneRequest, CancellationToken? cancellationToken = null);


        /// <summary>
        /// List your organization's fine-tuning jobs.
        /// </summary>
        /// <remarks>
        /// <para>See <seealso cref="https://api.openai.com/v1/fine-tunes">List fine-tunes</seealso>.</para>
        /// </remarks>
        /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
        /// <returns><see cref="ChatGPTFileInfo">A list of fine-tunes.</see></returns>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTListResponse<ChatGPTFineTuneJob>?> ListFineTunesAsync(CancellationToken? cancellationToken = null);

        /// <summary>
        /// Gets info about the fine-tune job.
        /// </summary>
        /// <remarks>
        /// <para>See <seealso cref="https://beta.openai.com/docs/api-reference/fine-tunes/retrieve">Retrieve fine-tunes</seealso>.</para>
        /// </remarks>
        /// <param name="fineTuneId">The ID of the fine-tune job.</param>
        /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
        /// <returns>The fine tune requested.</returns>
        /// <exception cref="ArgumentException">fineTuneId cannot be null or whitespace</exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTFineTuneJob?> RetrieveFineTuneAsync(string? fineTuneId, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Immediately cancel a fine-tune job.
        /// </summary>
        /// <remarks>
        /// <para>See <seealso cref="https://beta.openai.com/docs/api-reference/fine-tunes/cancel">Cancel fine-tunes</seealso>.</para>
        /// </remarks>
        /// <param name="fineTuneId">The ID of the fine-tune job to cancel.</param>
        /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
        /// <returns>The fine tune after it is cancelled.</returns>
        /// <exception cref="ArgumentException">fineTuneId cannot be null or whitespace</exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTFineTuneJob?> CancelFineTuneAsync(string? fineTuneId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Get fine-grained status updates for a fine-tune job.
        /// </summary>
        /// <param name="fineTuneId">The ID of the fine-tune job to get events for.</param>
        /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
        /// <returns>A list of events associated with the fineTuneId</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTListResponse<ChatGPTEvent>?> ListFineTuneEventsAsync(string? fineTuneId, CancellationToken? cancellationToken = null);


        /// <summary>
        /// <para>Given a input text(s), outputs if the model classifies it as violating OpenAI's content policy.</para>
        /// <para>Related guide: <see href="https://beta.openai.com/docs/guides/moderation">Moderations</see></para>
        /// </summary>
        /// <param name="createModerationRequest">Classifies if text violates OpenAI's Content Policy</param>
        /// <param name="cancellationToken">Optional. Propagates notifications that opertions should be cancelled.</param>
        /// <returns>A moderation response.</returns>
        /// <exception cref="ArgumentNullException"><c>CreateModerationAsync</c> requires an non-null <c>createModerationRequest</c></exception>
        /// <exception cref="ArgumentException"><c>CreateModerationAsync</c> requires an <c>Inputs</c></exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTCreateModerationResponse?> CreateModerationAsync(ChatGPTCreateModerationRequest? createModerationRequest, CancellationToken? cancellationToken = null);


        /// <summary>
        /// <para>Get a vector representation of a given input that can be easily consumed by machine learning models and algorithms.</para>
        /// <para>Related guide: <see href="https://beta.openai.com/docs/guides/embeddings">Embeddings</see></para>
        /// </summary>
        /// <param name="createEmbeddingsRequest">Input text to get embeddings for, encoded as a string or array of tokens. To get embeddings for multiple inputs in a single request, pass an array of strings or array of token arrays. Each input must not exceed 8192 tokens in length.</param>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        /// <returns>An embedding vector.</returns>
        /// <exception cref="ArgumentNullException">createEmbeddingsRequest cannot be null.</exception>
        /// <exception cref="ArgumentException">Model and Inputs are required.</exception>
        Task<ChatGPTCreateEmbeddingsResponse?> CreateEmbeddingsAsync(ChatGPTCreateEmbeddingsRequest? createEmbeddingsRequest, CancellationToken? cancellationToken = null);


        /// <summary>
        /// <para>Given a prompt and/or an input image, the model will generate a new image.</para>
        /// <para>Related guide: <see href="https://beta.openai.com/docs/guides/images">Image generation</see></para>
        /// </summary>
        /// <param name="createImageRequest">Creates one or more images given a text prompt.</param>
        /// <param name="cancellationToken">Optional. Propagates notifications that opertions should be cancelled.</param>
        /// <returns>Returns generated images as either urls or base64 encoded data depending on the ResponseFormat.</returns>
        /// <exception cref="ArgumentNullException"><c>CreateImageAsync</c> requires an non-null <c>createImageRequest</c></exception>
        /// <exception cref="ArgumentException"><c>CreateImageAsync</c> requires a <c>Prompt</c> and the number of images requested defaults to 1 (1-10).</exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTImageResponse?> CreateImageAsync(ChatGPTCreateImageRequest? createImageRequest, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Creates a variation of a given image.
        /// </summary>
        /// <param name="imageVariationRequest">Provide an image to be used for the variation.</param>
        /// <param name="cancellationToken">Optional. Propagates notifications that opertions should be cancelled.</param>
        /// <returns>One or more image variations.</returns>
        /// <exception cref="ArgumentNullException">imageVariationRequest cannot be null.</exception>
        /// <exception cref="ArgumentException">imageVariationRequest must include a file.</exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTImageResponse?> CreateImageVariationAsync(ChatGPTCreateImageVariationRequest? imageVariationRequest, CancellationToken? cancellationToken = null);


        /// <summary>
        /// <para>Creates an edited or extended image given an original image and a prompt.</para>
        /// <para>See <see href="https://beta.openai.com/docs/api-reference/images/create-edit">Create Edit</see></para>
        /// </summary>
        /// <param name="imageEditRequest">Includes an image and a prompt. A mask is optional</param>
        /// <param name="cancellationToken">Optional. Propagates notifications that opertions should be cancelled.</param>
        /// <returns>Edited image(s) generated from edit request.</returns>
        /// <exception cref="ArgumentNullException">imageEdit request is required</exception>
        /// <exception cref="ArgumentException">Prompt and Image is required. NumberofImagestoGenerate is 1-10. If Mask is not null, then the FileName and Contents are required.</exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception>
        Task<ChatGPTImageResponse?> CreateImageEditAsync(ChatGPTCreateImageEditRequest? imageEditRequest, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Creates a streamed completion for the provided prompt and parameters.
        /// </summary>
        /// <remarks>
        /// <para>Use the IAsyncEnumerable to process the response.</para>
        /// <para>See <seealso cref="https://beta.openai.com/docs/api-reference/completions/create">Create completion</seealso>.</para>
        /// </remarks>
        /// <param name="completionRequest">A well-defined prompt for requesting a completion.</param>
        /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
        /// <returns>An IAsyncEnumerable that streams the completion responses.</returns>
        /// <exception cref="ArgumentNullException">completionRequest is required.</exception>
        /// <exception cref="ArgumentException">Model is required.</exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception> 
        IAsyncEnumerable<ChatGPTCompletionResponse?> StreamCompletionAsync(ChatGPTCompletionRequest completionRequest, CancellationToken? cancellationToken = null);


        /// <summary>
        /// Streams the fine tune events for a given fine tune job.
        /// </summary>
        /// <param name="fineTuneId">Id of a submitted fine tune job.</param>
        /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
        /// <returns>An async enumerable that returns fine tune events as they are reported.</returns>
        /// <exception cref="ArgumentException">Requires finetuneid</exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception> 
        IAsyncEnumerable<ChatGPTEvent?> StreamFineTuneEventsAsync(string? fineTuneId, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Retrieves a byt[] of a generated image.
        /// </summary>
        /// <param name="generatedImage">Generated image returned from image create, edit, and variation calls.</param>
        /// <param name="cancellationToken">Propagates notifications that opertions should be cancelled.</param>
        /// <returns>A byte array of the downloaded image. Can be saved as a PNG.</returns>
        /// <exception cref="ArgumentNullException">generatedImage must have either a Url or Base64. Url must be a valid Uri.</exception>
        /// <exception cref="ArgumentException">Requires generatedImage</exception>
        /// <exception cref="ChatGPTException">Exception generated while processing request.</exception> 
        Task<byte[]?> DownloadImageAsync(GeneratedImage generatedImage, CancellationToken? cancellationToken = null);

        /// <summary>
        /// Apply new credentails.
        /// </summary>
        public ChatGPTCredentials? Credentials { set; }
    }
}