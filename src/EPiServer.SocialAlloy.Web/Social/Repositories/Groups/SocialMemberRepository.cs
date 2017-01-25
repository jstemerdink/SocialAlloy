﻿using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using EPiServer.SocialAlloy.ExtensionData.Membership;
using EPiServer.SocialAlloy.Web.Social.Adapters.Groups;
using EPiServer.SocialAlloy.Web.Social.Common.Exceptions;
using EPiServer.SocialAlloy.Web.Social.Models;
using EPiServer.SocialAlloy.Web.Social.Models.Groups;
using System.Collections.Generic;
using System.Linq;

namespace EPiServer.SocialAlloy.Web.Social.Repositories
{
    /// <summary>
    /// Defines the operations that can be issued against the EPiServer.Social.Groups.MemberService.
    /// </summary>
    public class SocialMemberRepository : ISocialMemberRepository
    {
        private readonly IMemberService memberService;
        private SocialMemberAdapter socialMemberAdapter;

        /// <summary>
        /// Constructor
        /// </summary>
        public SocialMemberRepository(IMemberService memberService)
        {
            this.memberService = memberService;
            this.socialMemberAdapter = new SocialMemberAdapter();
        }

        /// <summary>
        /// Adds a member to the EPiServer Social member repository.
        /// </summary>
        /// <param name="socialMember">The member to add.</param>
        /// <param name="memberExtension">The member extension data to add.</param>
        /// <returns>The added member.</returns>
        public SocialMember Add(SocialMember socialMember)
        {
            SocialMember addedSocialMember = null;

            try
            {
                var userReference = Reference.Create(socialMember.UserReference);
                var groupId = GroupId.Create(socialMember.GroupId);
                var member = new Member(userReference, groupId);
                var extensionData = new MemberExtensionData(socialMember.Email, socialMember.Company);
                var addedCompositeMember = this.memberService.Add<MemberExtensionData>(member, extensionData);
                addedSocialMember = socialMemberAdapter.Adapt(addedCompositeMember.Data, addedCompositeMember.Extension);
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with EPiServer social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for EPiServer Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with EPiServer Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("EPiServer Social failed to process the application request.", ex);
            }

            return addedSocialMember;
        }

        /// <summary>
        /// Retrieves a page of members from the EPiServer Social member repository.
        /// </summary>
        /// <param name="socialMemberFilter">The filter by which to retrieve members by</param>
        /// <returns>The list of members that are part of the specified group.</returns>
        public IEnumerable<SocialMember> Get(SocialMemberFilter socialMemberFilter)
        {
            IEnumerable<SocialMember> returnedMembers = null;

            try
            {
                var pageInfo = new PageInfo { PageSize = socialMemberFilter.PageSize };
                var memberFilter = new MemberFilter { Group = GroupId.Create(socialMemberFilter.GroupId) };
                var orderBy = new List<SortInfo> { new SortInfo(MemberSortFields.Id, false) };

                var compositeFilter = new CompositeCriteria<MemberFilter, MemberExtensionData>()
                {
                    Filter = memberFilter,
                    PageInfo = pageInfo,
                    OrderBy = orderBy
                };

                var compositeMember = this.memberService.Get(compositeFilter).Results;
                returnedMembers = compositeMember.Select(x => socialMemberAdapter.Adapt(x.Data, x.Extension));
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with EPiServer social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for EPiServer Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with EPiServer Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("EPiServer Social failed to process the application request.", ex);
            }

            return returnedMembers;
        }
    }
}