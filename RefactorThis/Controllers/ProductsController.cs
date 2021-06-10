using System;
using System.Collections.Generic;
using System.Linq;
using DB.Repository.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RefactorThis.DB.Entity;
using RefactorThis.DB.Repository;
using RefactorThis.Mapper.Interface;
using RefactorThis.Models;

namespace RefactorThis.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductOptionRepository _productOptionRepository;
        private readonly IProductOptionMapper _productOptionMapper;
        private readonly IProductMapper _productMapper; 
        private readonly IUnitOfWork _unitOfWork;
        public ProductsController(
            IUnitOfWork unitOfWork,
            IProductMapper mapper,
            IProductOptionMapper productOptionMapper,
            IProductOptionRepository productOptionRepository,
            ILogger logger): base(logger)
        {
            _productRepository = unitOfWork.ProductRepository;
            _productOptionMapper = productOptionMapper;
            _productOptionRepository = unitOfWork.ProductOptionRepository;
            _productMapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        // TODO better to make the action results async
        public ActionResult<ProductListDto> Get([FromQuery(Name = "name")] string name)
        {
            try
            {
                IEnumerable<Product> result;

                result = name != null ? _productRepository.Get(p => p.Name == name, "ProductOptions") : _productRepository.Get(null, "ProductOptions");

                var productDtos = _productMapper.ToDtoList(result);
                 return Ok(productDtos);
            }

            catch (Exception ex)
            {
                return ExceptionHandler(ex);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Product> Get(Guid id)
        {
            try
            {
                var result = _productRepository.GetById(id);
                
                if(result != null)
                {
                    var productDto = _productMapper.ToDto(result);
                    return Ok(productDto);
                }
                else
                {
                    return NoContent();
                }
               
            }

            catch (Exception ex)
            {
                return ExceptionHandler(ex);
            }
        }

        [HttpPost]
        public ActionResult<Guid> Post(ProductDto productDto)
        {
            try
            {
                var product = _productMapper.ToEntity(productDto, Guid.Empty);
                var result = _productRepository.Insert(product);
                _unitOfWork.Save();
                return Ok(result);
            }

            catch (Exception ex)
            {
                return ExceptionHandler(ex);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Update(Guid id, ProductDto productDto)
        {
            try
            {
                var product = _productMapper.ToEntity(productDto, id);
                _productRepository.Update(product);
                _unitOfWork.Save();
                return Ok(true);
            }

            catch (Exception ex)
            {
                return ExceptionHandler(ex);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            try
            {
                _productRepository.Delete(id);
                _unitOfWork.Save();
                return Ok(true);
            }

            catch (Exception ex)
            {
                return ExceptionHandler(ex);
            }
        }

        // TODO: I would like to move this to a separate controller,
        // But I left it here as I want to meet the readme file

        [HttpGet("{id}/options")]
        public ActionResult<List<ProductOptionDto>> GetOptions(Guid id)
        {
            try
            {
                var result = _productOptionRepository.Get(po => po.ProductId == id, "");

                if(result != null)
                {
                    var productOptionDtos = _productOptionMapper.ToDtoList(result);
                    return Ok(productOptionDtos);
                }
                else
                {
                    return NoContent();
                }

            }

            catch (Exception ex)
            {
                return ExceptionHandler(ex);
            }
        }


        [HttpGet("{id}/options/{optionId}")]
        public ActionResult<ProductOptionDto> GetOptions(Guid id, Guid optionId)
        {
            try
            {
                var result = _productOptionRepository.Get(po => po.ProductId == id && po.Id == optionId, "").FirstOrDefault();
                if(result != null)
                {
                    var productOptionDto = _productOptionMapper.ToDto(result);
                    return Ok(productOptionDto);
                }
                else
                {
                    return NoContent();
                }

            }

            catch (Exception ex)
            {
                return ExceptionHandler(ex);
            }
        }

        [HttpPost("{id}/options")]
        public ActionResult Post(Guid id, ProductOptionDto option)
        {
            try
            {                
                var productOption = _productOptionMapper.ToEntity(option, Guid.Empty, id);
                var result = _productOptionRepository.Insert(productOption);
                _unitOfWork.Save();
                return Ok(result);
            }

            catch (Exception ex)
            {
                return ExceptionHandler(ex);
            }
        }

        [HttpPut("{id}/options/{optionId}")]
        public ActionResult UpdateOption(Guid id, Guid optionId, ProductOptionDto option)
        {
            try
            {
                var productOption = _productOptionMapper.ToEntity(option, optionId, id);
                _productOptionRepository.Update(productOption);
                _unitOfWork.Save();
                return Ok(true);
            }

            catch (Exception ex)
            {
                return ExceptionHandler(ex);
            }
        }

        [HttpDelete("{id}/options/{optionId}")]
        public ActionResult DeleteOption(Guid id)
        {
            try
            {
                _productOptionRepository.Delete(id);
                _unitOfWork.Save();
                return Ok(true);
            }

            catch (Exception ex)
            {
                return ExceptionHandler(ex);
            }
        }
    }
}