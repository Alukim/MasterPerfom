using Nest;

namespace MasterPerform.Infrastructure.Elasticsearch.Mappings.Descriptors
{
    public static class TokenizersDescriptors
    {
        public static TokenizersDescriptor NGram(this TokenizersDescriptor descriptor)
            => descriptor.NGram(CustomTokenizers.NGRAM, z => z
                .MinGram(1)
                .MaxGram(30)
                .TokenChars(
                    TokenChar.Letter,
                    TokenChar.Digit,
                    TokenChar.Punctuation,
                    TokenChar.Symbol,
                    TokenChar.Whitespace
                ));
    }
}
