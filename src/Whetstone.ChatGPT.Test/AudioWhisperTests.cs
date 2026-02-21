// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Whetstone.ChatGPT.Models.Audio;
using Whetstone.ChatGPT.Models.File;
using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace Whetstone.ChatGPT.Test
{
    public class AudioWhisperTests
    {
        [Fact(Skip = "reduce testing costs")]
        public async Task TranscribeFileAsync()
        {
            ChatGPTAudioTranscriptionRequest uploadRequest = new ChatGPTAudioTranscriptionRequest
            {
                File = GetAudioFileContent()
            };
            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                ChatGPTAudioResponse? audioResponse = await client.CreateTranscriptionAsync(uploadRequest, true);
                string? text = audioResponse?.Text;

                Assert.NotNull(text);
            }
        }

        [Fact(Skip = "reduce testing costs")]
        public async Task TranscribeFileToTextAsync()
        {
            ChatGPTAudioTranscriptionRequest uploadRequest = new ChatGPTAudioTranscriptionRequest
            {
                File = GetAudioFileContent()
            };

            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                string? textReponse = await client.CreateTranscriptionAsync(uploadRequest, AudioResponseFormatText.WebVtt);
                Assert.NotNull(textReponse);
            }
        }

        [Fact(Skip = "reduce testing costs")]
        public async Task TranslateFileAsync()
        {
            ChatGPTAudioTranslationRequest uploadRequest = new ChatGPTAudioTranslationRequest
            {
                File = GetAudioFileContent()
            };
            
            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                ChatGPTAudioResponse? audioResponse = await client.CreateTranslationAsync(uploadRequest, true);
                string? text = audioResponse?.Text;
                Assert.NotNull(text);
            }
        }

        [Fact(Skip = "reduce testing costs")]
        public async Task TranslateFileToTextAsync()
        {
            ChatGPTAudioTranslationRequest uploadRequest = new ChatGPTAudioTranslationRequest
            {
                File = GetAudioFileContent()
            };
            
            using (IChatGPTClient client = ChatGPTTestUtilties.GetClient())
            {
                string? textReponse = await client.CreateTranslationAsync(uploadRequest, AudioResponseFormatText.WebVtt);
                Assert.NotNull(textReponse);
            }
        }

        private ChatGPTFileContent GetAudioFileContent()
        {
            string audioFile = @"audiofiles\transcriptiontest.mp3";

            byte[] fileContents = File.ReadAllBytes(audioFile);
            ChatGPTFileContent gptFile = new ChatGPTFileContent
            {
                FileName = audioFile,
                Content = fileContents
            };

            return gptFile;
        }
    }
}
